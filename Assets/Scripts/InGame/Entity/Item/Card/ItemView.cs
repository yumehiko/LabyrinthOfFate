using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.View
{
    public class ItemView : IItemView
    {
        public string Name { get; }
        public Sprite Frame { get; }
        public string InvokeEffect { get; }
        public string StatsInfo { get; }

        public ItemView(string name, Sprite frame, string invokeEffect, string statsInfo)
        {
            this.Name = name;
            this.Frame = frame;
            this.InvokeEffect = invokeEffect;
            this.StatsInfo = statsInfo;
        }
    }
}