using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.Invoke
{
    public class CantInvoke : IInvokeEffect
    {
        public InvokeType Type => InvokeType.CantInvoke;

        public string Info =>
            $"Invoke: You Can't Invoke this Card.";

        public void Invoke(IActorModel user, IItemModel parent)
        {

        }
    }
}