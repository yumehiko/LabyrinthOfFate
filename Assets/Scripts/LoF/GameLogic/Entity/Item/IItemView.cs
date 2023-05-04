using UnityEngine;

namespace LoF.GameLogic.Entity.Item
{
    public interface IItemView
    {
        string Name { get; }
        Sprite Frame { get; }
        string InvokeEffectInfo { get; }
        string StatsInfo { get; }
        bool CanInvoke { get; }
    }
}