using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using VContainer;
using System.Linq;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// プレイヤーのためのインベントリUI。そのプレゼンター。
    /// </summary>
    public class InventoryUI : IDisposable
    {
        public IReadOnlyReactiveProperty<bool> IsOpen => isOpen;
        public IObservable<InventoryCommand> OnCommand => onCommand;
        public InventoryModel Model => model;
        public bool IsFull => model.IsFull;

        private readonly Subject<InventoryCommand> onCommand = new Subject<InventoryCommand>();
        private readonly BoolReactiveProperty isOpen = new BoolReactiveProperty();
        private readonly InventoryUIView view;
        private InventoryModel model;
        private CancellationTokenSource openCancellation = null;
        private IDisposable refleshDisposable;

        [Inject]
        public InventoryUI(InventoryUIView view)
        {
            this.view = view;
            view.Initialize();
        }

        /// <summary>
        /// モデルを登録し、モデルに応じてカードインベントリUIを更新する。
        /// </summary>
        /// <param name="model"></param>
        public void Initialize(InventoryModel model)
        {
            this.model = model;
            refleshDisposable = model.OnReflesh.Subscribe(_ => RefleshView());
            RefleshView();
        }

        public void Dispose()
        {
            refleshDisposable.Dispose();
            if (openCancellation == null) return;
            openCancellation.Cancel();
            openCancellation.Dispose();
        }

        public void SwitchOpen()
        {
            if (isOpen.Value)
            {
                Close();
            }
            else
            {
                Open().Forget();
            }
        }

        public async UniTask Open()
        {
            openCancellation?.Dispose();
            openCancellation = new CancellationTokenSource();
            isOpen.Value = true;
            var command = await view.Open(openCancellation.Token);

            if (command.Type == InventoryCommandType.Cancel)
            {
                return;
            }
            onCommand.OnNext(command); //コマンドの実際の発動は親（Player）が実行する。
        }

        public void Close()
        {
            isOpen.Value = false;
            openCancellation?.Cancel();
        }

        public static IItemView MakeView(IItemModel model)
        {
            var name = model.Name;
            var frame = model.Frame;
            var invokeEffect = model.InvokeEffect;
            var statsInfo = model.StatsInfo;
            var view = new ItemView(name, frame, invokeEffect, statsInfo);
            return view;
        }

        public void EquipAsWeaponCommand(SlotType type, int slotID)
        {
            if (type == SlotType.Armor)
            {
                model.SwitchWeaponArmor();
                return;
            }

            if (type == SlotType.Inventory)
            {
                model.EquipAsWeaponFromInventory(slotID);
                return;
            }

            throw new Exception($"武器の交換に失敗した。");
        }

        public void EquipAsArmorCommand(SlotType type, int slotID)
        {
            //武器と防具を交換
            if (type == SlotType.Weapon)
            {
                model.SwitchWeaponArmor();
                return;
            }

            //アイテムと防具を交換。
            if (type == SlotType.Inventory)
            {
                model.EquipAsArmorFromInventory(slotID);
                return;
            }

            throw new Exception($"防具の交換に失敗した。");
        }

        private void RefleshView()
        {
            var weapon = MakeView(model.EquipSlot.Weapon);
            var armor = MakeView(model.EquipSlot.Armor);
            var itemViews = new List<IItemView>();
            foreach (IItemModel item in model)
            {
                var itemView = MakeView(item);
                itemViews.Add(itemView);
            }

            view.RefleshView(weapon, armor, itemViews);
        }
    }
}