using System;
using System.Collections;
using System.Collections.Generic;
using LoF.GameLogic.Entity.Item;
using LoF.GameLogic.Entity.Item.Card;
using UniRx;

namespace LoF.GameLogic.Entity.Actor.Model
{
    /// <summary>
    ///     アイテムのコレクション。
    /// </summary>
    public class Inventory : IEnumerable<IItemModel>
    {
        private readonly List<IItemModel> items = new();

        private readonly Subject<Unit> onReflesh = new();
        public int MaxCapacity = 5;

        public Inventory(IActorProfile profile)
        {
            var weapon = new CardModel(profile.Weapon);
            var armor = new CardModel(profile.Armor);
            EquipSlot = new EquipSlot(weapon, armor);
            foreach (var cardProfile in profile.InventoryCards)
            {
                var card = new CardModel(cardProfile);
                Add(card);
            }
        }

        public EquipSlot EquipSlot { get; }
        public IItemModel this[int index] => items[index];
        public int Count => items.Count;
        public bool IsFull => items.Count == MaxCapacity;
        public IObservable<Unit> OnReflesh => onReflesh;

        IEnumerator<IItemModel> IEnumerable<IItemModel>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        ///     アイテムを追加する。
        ///     追加できた場合、インベントリ上の番号を返す。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add(IItemModel item)
        {
            if (items.Count >= MaxCapacity) throw new Exception("インベントリの容量を超えてしまう");
            items.Add(item);
            onReflesh.OnNext(Unit.Default);
            return items.Count - 1;
        }

        public void EquipWeapon(ICardModel card)
        {
            EquipSlot.EquipWeapon(card);
            onReflesh.OnNext(Unit.Default);
        }

        public void EquipArmor(ICardModel card)
        {
            EquipSlot.EquipArmor(card);
            onReflesh.OnNext(Unit.Default);
        }

        public void Remove(IItemModel item)
        {
            items.Remove(item);
            onReflesh.OnNext(Unit.Default);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            onReflesh.OnNext(Unit.Default);
        }

        public void InvokeCard(int id, IActorModel user)
        {
            items[id].InvokeEffect.Invoke(user, items[id]);
        }

        /// <summary>
        ///     インベントリから選んで武器を装備する。
        /// </summary>
        /// <param name="id"></param>
        public void EquipAsWeaponFromInventory(int id)
        {
            var originWeapon = EquipSlot.Weapon;
            var target = (ICardModel)items[id];
            items.RemoveAt(id);
            EquipSlot.EquipWeapon(target);
            items.Add(originWeapon);
            onReflesh.OnNext(Unit.Default);
        }

        /// <summary>
        ///     インベントリから選んで防具を装備する。
        /// </summary>
        /// <param name="id"></param>
        public void EquipAsArmorFromInventory(int id)
        {
            var originArmor = EquipSlot.Armor;
            var target = (ICardModel)items[id];
            items.RemoveAt(id);
            EquipSlot.EquipArmor(target);
            items.Add(originArmor);
            onReflesh.OnNext(Unit.Default);
        }

        /// <summary>
        ///     武器と防具を交換する。
        /// </summary>
        public void SwitchWeaponArmor()
        {
            var originArmor = EquipSlot.Armor;
            var originWeapon = EquipSlot.Weapon;
            EquipSlot.EquipWeapon(originArmor);
            EquipSlot.EquipArmor(originWeapon);
            onReflesh.OnNext(Unit.Default);
        }

        public int IndexOf(IItemModel item)
        {
            return items.IndexOf(item);
        }
    }
}