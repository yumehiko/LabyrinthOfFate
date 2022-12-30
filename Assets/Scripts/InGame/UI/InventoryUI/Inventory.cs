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
using yumehiko.LOF.Invoke;

namespace yumehiko.LOF.Presenter
{
    public class Inventory : IDisposable
    {
        public IReadOnlyReactiveProperty<bool> IsOpen => isOpen;
        public IObservable<Unit> OnCommand => onCommand;
        public IObservable<IItemModel> OnInvokeEffect => onInvokeEffect;

        private readonly Subject<Unit> onCommand = new Subject<Unit>();
        private readonly Subject<IItemModel> onInvokeEffect = new Subject<IItemModel>();

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
        /// <param name="item"></param>
        public void AddItem(IItemModel item)
        {
            try
            {
                var itemView = MakeView(item);
                int id = model.Add(item);
                view.Add(itemView);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveItem(IItemModel item)
        {
            int id = model.IndexOf(item);
            view.RemoveAt(id);
            model.RemoveAt(id);
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
            DoCommand(command);
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

        private void DoCommand(InventoryCommand command)
        {
            switch (command.Type)
            {
                case InventoryCommandType.Invoke:
                    InvokeCard(command.Slot);
                    break;
                case InventoryCommandType.EquipAsWeapon:
                    EquipAsWeaponCommand(command.Slot);
                    break;
                case InventoryCommandType.EquipAsArmor:
                    EquipAsArmorCommand(command.Slot);
                    break;
                case InventoryCommandType.Cancel: //何もしない
                    break;
                default: throw new Exception($"不正なインベントリコマンド：{command.Type}");
            }
        }

        private void InvokeCard(InventorySlotView slotView)
        {
            //処理は所有者側で行う。
            int id = slotView.ID;
            onInvokeEffect.OnNext(model[id]);
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
                onCommand.OnNext(Unit.Default);
                return;
            }

            if (slotView.Type == SlotType.Inventory)
            {
                int id = slotView.ID;
                weapon = (ICardModel)model[id];
                model.RemoveAt(id);
                view.RemoveAt(id);
                model.EquipWeapon(weapon);
                view.SetWeapon(MakeView(weapon));
                model.Add(origin);
                view.Add(MakeView(origin));
                onCommand.OnNext(Unit.Default);
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
                onCommand.OnNext(Unit.Default);
                return;
            }

            //アイテムと防具を交換。
            if (slotView.Type == SlotType.Inventory)
            {
                int id = slotView.ID;
                armor = (ICardModel)model[id];
                model.RemoveAt(id);
                view.RemoveAt(id);
                model.EquipArmor(armor);
                view.SetArmor(MakeView(armor));
                model.Add(origin);
                view.Add(MakeView(origin));
                onCommand.OnNext(Unit.Default);
                return;
            }

            throw new Exception($"防具の交換に失敗した。");
        }
    }
}