using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムのコレクション。
    /// </summary>
    public class Inventory
    {
        private readonly List<IItem> items = new List<IItem>();

        public readonly int Capacity = 5;
        public IEnumerator<IItem> GetEnumerator() => items.GetEnumerator();

        public void Add(IItem item)
        {
            items.Add(item);
        }
    }
}