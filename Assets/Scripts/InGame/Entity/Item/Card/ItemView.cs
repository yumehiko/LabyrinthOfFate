using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.Invoke;

namespace yumehiko.LOF.View
{
    public class ItemView : IItemView
    {
        public string Name { get; }
        public Sprite Frame { get; }
        public string InvokeEffectInfo { get; }
        public string StatsInfo { get; }
        public bool CanInvoke { get; }

        public ItemView(string name, Sprite frame, IInvokeEffect invokeEffect, string statsInfo)
        {
            this.Name = name;
            this.Frame = frame;
            this.InvokeEffectInfo = invokeEffect.Info;
            this.StatsInfo = statsInfo;
            CanInvoke = invokeEffect.Type != InvokeType.CantInvoke;
        }
    }
}