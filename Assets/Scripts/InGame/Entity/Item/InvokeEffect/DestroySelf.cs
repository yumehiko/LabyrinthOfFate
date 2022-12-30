using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Presenter;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.Invoke
{
    /// <summary>
    /// このカードを破壊する。
    /// </summary>
    public class DestroySelf : IInvokeEffect
    {
        public InvokeType Type => InvokeType.DestroySelf;
        public string Info =>
            $"Invoke: Destroy this Card.";

        public void Invoke(Player player, IItemModel parent)
        {
            player.Inventory.RemoveItem(parent);
        }
    }
}