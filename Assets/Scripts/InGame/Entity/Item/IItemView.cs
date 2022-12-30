using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.View
{
    public interface IItemView
    {
        string Name { get; }
        Sprite Frame { get; }
        string InvokeEffectInfo { get; }
        string StatsInfo { get; }
    }
}