using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.View
{
    public class CardInfo
    {
        public string Name { get; }
        public string Stats { get; }
        public string InvokeEffect { get; }
        public Sprite Frame { get; }

        public CardInfo(
            string name,
            string stats,
            string invokeEffect,
            Sprite frame)
        {
            Name = name;
            Stats = stats;
            InvokeEffect = invokeEffect;
            Frame = frame;
        }
    }
}