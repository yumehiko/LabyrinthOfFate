using LoF.GameLogic.Entity.Item.InvokeEffect;
using UnityEngine;

namespace LoF.GameLogic.Entity.Item.Card
{
    public class ItemView : IItemView
    {
        public ItemView(string name, Sprite frame, IInvokeEffect invokeEffect, string statsInfo)
        {
            Name = name;
            Frame = frame;
            InvokeEffectInfo = invokeEffect.Info;
            StatsInfo = statsInfo;
            CanInvoke = invokeEffect.Type != InvokeType.CantInvoke;
        }

        public string Name { get; }
        public Sprite Frame { get; }
        public string InvokeEffectInfo { get; }
        public string StatsInfo { get; }
        public bool CanInvoke { get; }
    }
}