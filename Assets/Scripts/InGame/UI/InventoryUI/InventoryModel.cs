using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムのコレクション。
    /// </summary>
    public class InventoryModel
    {
        public ICardModel Weapon => equipSlot.Weapon;
        public ICardModel Armor => equipSlot.Armor;
        public IItemModel this[int index] => items[index];
        public IReadOnlyReactiveProperty<int> Count => count;
        public int Capacity = 5;
        public bool IsFull => count.Value == Capacity;

        private readonly IntReactiveProperty count = new IntReactiveProperty();
        private readonly List<IItemModel> items = new List<IItemModel>();
        private EquipSlot equipSlot;

        /// <summary>
        /// アイテムを追加する。
        /// 追加できた場合、インベントリ上の番号を返す。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add(IItemModel item)
        {
            if (items.Count >= Capacity)
            {
                throw new Exception("インベントリの容量を超えてしまう");
            }
            items.Add(item);
            count.Value = items.Count;
            return items.Count - 1;
        }

        public int IndexOf(IItemModel item) => items.IndexOf(item);
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            count.Value = items.Count;
        }

        public void InitializeEquipSlot(EquipSlot equipSlot) => this.equipSlot = equipSlot;
        public void EquipWeapon(ICardModel card) => equipSlot.EquipWeapon(card);
        public void EquipArmor(ICardModel card) => equipSlot.EquipArmor(card);
    }
}