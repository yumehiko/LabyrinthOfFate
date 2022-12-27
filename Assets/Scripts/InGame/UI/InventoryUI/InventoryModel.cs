using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムのコレクション。
    /// </summary>
    public class InventoryModel
    {
        public int Count => items.Count;
        public readonly int Capacity = 5;
        public IEnumerator<IItemModel> GetEnumerator() => items.GetEnumerator();

        private IItemModel weaponSlot;
        private IItemModel armorSlot;
        private readonly List<IItemModel> items = new List<IItemModel>();

        /// <summary>
        /// アイテムを追加する。
        /// 追加できた場合、インベントリ上の番号を返す。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add(IItemModel item)
        {
            if(items.Count >= 5)
            {
                throw new Exception("インベントリの容量を超えてしまう");
            }
            int id = Count;
            items.Add(item);
            return id;
        }

        public void SetWeapon(IItemModel card)
        {
            //TODO:ITEMのタイプをチェックする必要がある
            weaponSlot = card;
        }

        public void SetArmor(IItemModel card)
        {
            armorSlot = card;
        }
    }
}