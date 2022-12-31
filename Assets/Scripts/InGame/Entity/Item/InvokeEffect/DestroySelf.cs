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

        public void Invoke(IActorModel user, IItemModel parent)
        {
            user.Inventory.Remove(parent);
            var userName = user.ActorType == ActorType.Player ? "You" : user.Name;
            var message = $"{userName} tore up the card.";
            var result = new RawResult(message);
            user.SendResultMessage(result);
        }
    }
}