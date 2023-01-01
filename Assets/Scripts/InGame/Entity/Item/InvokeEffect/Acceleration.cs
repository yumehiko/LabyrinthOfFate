using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Presenter;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.Invoke
{
    /// <summary>
    /// このフロアにいる間、加速する。
    /// </summary>
    public class Acceleration : IInvokeEffect
    {
        public InvokeType Type => InvokeType.Acceleration;
        public string Info =>
            $"Invoke: Accelerate while on this floor.";

        public void Invoke(IActorModel user, IItemModel parent)
        {
            var buff = new AccelerateBuff();
            user.Status.Buffs.Add(buff);

            user.Inventory.Remove(parent);
            var userName = user.ActorType == ActorType.Player ? "You" : user.Name;
            var message = $"{userName} began to Accelerate!";
            var result = new RawResult(message);
            user.SendResultMessage(result);
        }
    }
}