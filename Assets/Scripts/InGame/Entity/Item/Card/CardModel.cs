using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using yumehiko.LOF.Invoke;

namespace yumehiko.LOF.Model
{
    public class CardModel : ICardModel
    {
        public ITemType Type => ITemType.Card;
        public string Name { get; }
        public Sprite Frame { get; }
        public IInvokeEffect InvokeEffect { get; }
        public string StatsInfo => GetInfo();
        public AttackStatuses AttackStatuses { get; }
        public DefenceStatus DefenceStatus { get; }

        public CardModel(CardProfile profile)
        {
            Name = profile.Name;
            Frame = profile.Frame;
            InvokeEffect = InvokeFactory(profile.InvokeType);
            AttackStatuses = new AttackStatuses(profile.AttackStatuses);
            DefenceStatus = new DefenceStatus(profile.DefenceStatus);
        }

        private string GetInfo()
        {
            var stats = AttackStatuses.GetInfo();
            stats += Environment.NewLine;
            stats += DefenceStatus.GetInfo();
            return stats;
        }

        private IInvokeEffect InvokeFactory(InvokeType type)
        {
            switch (type)
            {
                case InvokeType.DestroySelf: return new DestroySelf();
                default: throw new Exception($"未定義のInvokeEffectType");
            }
        }
    }
}