using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using VContainer;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace yumehiko.LOF.Presenter
{
    public class Inventory : IDisposable
    {
        public IReadOnlyReactiveProperty<bool> IsOpen => isOpen;
        public IObservable<Unit> OnCommand => onCommand;

        private readonly Subject<Unit> onCommand = new Subject<Unit>();
        private readonly BoolReactiveProperty isOpen = new BoolReactiveProperty();
        private readonly InventoryModel model;
        private readonly InventoryUI view;
        private CancellationTokenSource openCancellation = null;

        [Inject]
        public Inventory(InventoryModel model, InventoryUI view)
        {
            this.model = model;
            this.view = view;
            view.Initialize();
        }

        public void InitializeEquips(EquipSlot equipSlot)
        {
            model.InitializeEquipSlot(equipSlot);
            view.SetWeapon(MakeView(equipSlot.Weapon));
            view.SetArmor(MakeView(equipSlot.Armor));
        }

        public void Dispose()
        {
            if (openCancellation == null) return;
            openCancellation.Cancel();
            openCancellation.Dispose();
        }

        public bool CanAddItem() => model.Count < 5;

        /// <summary>
        /// アイテムをインベントリに新たに追加する。
        /// </summary>
        /// <param name="itemModel"></param>
        public void AddItem(IItemModel itemModel)
        {
            try
            {
                var itemView = MakeView(itemModel);
                int id = model.Add(itemModel);
                view.SetItem(itemView, id);
            }
            catch (Exception e)
            {
                throw e;
            }
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
            openCancellation = new CancellationTokenSource();
            isOpen.Value = true;
            var command = await view.Open(openCancellation.Token);
            DoCommand(command);
            Close();
        }

        public void Close()
        {
            isOpen.Value = false;
            if (openCancellation == null) return;
            openCancellation.Cancel();
            openCancellation.Dispose();
            openCancellation = null;
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

        private void DoCommand(InventoryCommand command)
        {
            Debug.Log($"Command:{command.Type}");

            switch (command.Type)
            {
                case InventoryCommandType.Invoke:
                    Debug.Log($"Invokeは未実装");
                    break;
                case InventoryCommandType.EquipAsWeapon:
                    EquipAsWeaponCommand(command.Slot);
                    break;
                case InventoryCommandType.EquipAsArmor:
                    EquipAsArmorCommand(command.Slot);
                    break;
                default: throw new Exception($"不正なインベントリコマンド：{command.Type}");
            }

            onCommand.OnNext(Unit.Default);
        }

        private void EquipAsWeaponCommand(InventorySlotView slotView)
        {
            ICardModel origin = model.Weapon;
            ICardModel weapon;

            if (slotView.Type == SlotType.Armor)
            {
                weapon = model.Armor;
                model.EquipWeapon(weapon);
                model.EquipArmor(origin);
                view.SetWeapon(MakeView(weapon));
                view.SetArmor(MakeView(origin));
                return;
            }

            if (slotView.Type == SlotType.Inventory)
            {
                int id = slotView.ID;
                weapon = (ICardModel)model[id];
                model.EquipWeapon(weapon);
                model.SetItemToSlot(origin, id);
                view.SetWeapon(MakeView(weapon));
                view.SetItem(MakeView(origin), id);
                return;
            }

            throw new Exception($"武器の交換に失敗した。");
        }

        private void EquipAsArmorCommand(InventorySlotView slotView)
        {
            ICardModel origin = model.Armor;
            ICardModel armor;

            //武器と防具を交換
            if (slotView.Type == SlotType.Weapon)
            {
                armor = model.Weapon;
                model.EquipArmor(armor);
                model.EquipWeapon(origin);
                view.SetArmor(MakeView(armor));
                view.SetWeapon(MakeView(origin));
                return;
            }

            //アイテムと防具を交換。
            if (slotView.Type == SlotType.Inventory)
            {
                int id = slotView.ID;
                armor = (ICardModel)model[id];
                model.EquipArmor(armor);
                model.SetItemToSlot(origin, id);
                view.SetArmor(MakeView(armor));
                view.SetItem(MakeView(origin), id);
                return;
            }

            throw new Exception($"防具の交換に失敗した。");
        }
    }
}