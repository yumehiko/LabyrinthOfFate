using System;
using LoF.GameLogic.Entity.Item.InvokeEffect;
using UnityEngine;

namespace LoF.GameLogic.Entity.Item.Card
{
    public class CardModel : ICardModel
    {
        public CardModel(CardProfile profile)
        {
            Name = profile.Name;
            Frame = profile.Frame;
            InvokeEffect = InvokeFactory(profile.InvokeType);
            AttackStatuses = new AttackStatuses(profile.AttackStatuses);
            DefenceStatus = new DefenceStatus(profile.DefenceStatus);
        }

        public ITemType Type => ITemType.Card;
        public string Name { get; }
        public Sprite Frame { get; }
        public IInvokeEffect InvokeEffect { get; }
        public string StatsInfo => GetInfo();
        public AttackStatuses AttackStatuses { get; }
        public DefenceStatus DefenceStatus { get; }

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
                case InvokeType.CantInvoke: return new CantInvoke();
                case InvokeType.DestroySelf: return new DestroySelf();
                case InvokeType.Acceleration: return new Acceleration();
                default: throw new Exception("未定義のInvokeEffectType");
            }
        }
    }
}