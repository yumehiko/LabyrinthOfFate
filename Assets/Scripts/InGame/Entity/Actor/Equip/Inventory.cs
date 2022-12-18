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
        public IEnumerator<IItem> GetEnumerator() => items.GetEnumerator();
    }
}