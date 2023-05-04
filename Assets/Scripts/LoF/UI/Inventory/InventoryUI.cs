using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LoF.GameLogic.Entity.Item;
using LoF.GameLogic.Entity.Item.Card;
using UniRx;
using VContainer;

namespace LoF.UI.Inventory
{
    /// <summary>
    ///     プレイヤーのためのインベントリUI。そのプレゼンター。
    /// </summary>
    public class InventoryUI : IDisposable
    {
        private readonly BoolReactiveProperty isOpen = new();

        private readonly Subject<InventoryCommand> onCommand = new();
        private readonly InventoryUIView view;
        private CancellationTokenSource openCancellation;
        private IDisposable refleshDisposable;

        [Inject]
        public InventoryUI(InventoryUIView view)
        {
            this.view = view;
            view.Initialize();
        }

        public IReadOnlyReactiveProperty<bool> IsOpen => isOpen;
        public IObservable<InventoryCommand> OnCommand => onCommand;
        public GameLogic.Entity.Actor.Model.Inventory Model { get; private set; }

        public bool IsFull => Model.IsFull;

        public void Dispose()
        {
            refleshDisposable.Dispose();
            if (openCancellation == null) return;
            openCancellation.Cancel();
            openCancellation.Dispose();
        }

        /// <summary>
        ///     モデルを登録し、モデルに応じてカードインベントリUIを更新する。
        /// </summary>
        /// <param name="model"></param>
        public void Initialize(GameLogic.Entity.Actor.Model.Inventory model)
        {
            this.Model = model;
            refleshDisposable = model.OnReflesh.Subscribe(_ => RefleshView());
            RefleshView();
        }

        public void SwitchOpen()
        {
            if (isOpen.Value)
                Close();
            else
                Open().Forget();
        }

        public async UniTask Open()
        {
            openCancellation?.Dispose();
            openCancellation = new CancellationTokenSource();
            isOpen.Value = true;
            var command = await view.Open(openCancellation.Token);

            if (command.Type == InventoryCommandType.Cancel) return;
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
                Model.SwitchWeaponArmor();
                return;
            }

            if (type == SlotType.Inventory)
            {
                Model.EquipAsWeaponFromInventory(slotID);
                return;
            }

            throw new Exception("武器の交換に失敗した。");
        }

        public void EquipAsArmorCommand(SlotType type, int slotID)
        {
            //武器と防具を交換
            if (type == SlotType.Weapon)
            {
                Model.SwitchWeaponArmor();
                return;
            }

            //アイテムと防具を交換。
            if (type == SlotType.Inventory)
            {
                Model.EquipAsArmorFromInventory(slotID);
                return;
            }

            throw new Exception("防具の交換に失敗した。");
        }

        private void RefleshView()
        {
            var weapon = MakeView(Model.EquipSlot.Weapon);
            var armor = MakeView(Model.EquipSlot.Armor);
            var itemViews = new List<IItemView>();
            foreach (IItemModel item in Model)
            {
                var itemView = MakeView(item);
                itemViews.Add(itemView);
            }

            view.RefleshView(weapon, armor, itemViews);
        }
    }
}