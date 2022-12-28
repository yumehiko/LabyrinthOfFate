using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムのコレクション。
    /// </summary>
    public class InventoryModel : IReadOnlyList<IItemModel>
    {
        public ICardModel Weapon => equipSlot.Weapon;
        public ICardModel Armor => equipSlot.Armor;
        public int Count => items.Count;
        public IItemModel this[int index] => items[index];
    
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
            if (items.Count >= 5)
            {
                throw new Exception("インベントリの容量を超えてしまう");
            }
            int id = Count;
            items.Add(item);
            return id;
        }

        /// <summary>
        /// スロットを指定してアイテムをセット。
        /// </summary>
        /// <param name="slotID"></param>
        /// <param name="item"></param>
        public void SetItemToSlot(IItemModel item, int slotID)
        {
            items[slotID] = item;
        }

        public void InitializeEquipSlot(EquipSlot equipSlot) => this.equipSlot = equipSlot;
        public void EquipWeapon(ICardModel card) => equipSlot.EquipWeapon(card);
        public void EquipArmor(ICardModel card) => equipSlot.EquipArmor(card);

        public IEnumerator<IItemModel> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }
}