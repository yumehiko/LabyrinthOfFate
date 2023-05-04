using System;
using LoF.GameLogic.Entity.Item.Card;
using LoF.UI.ResultMessage;
using UnityEngine;

namespace LoF.GameLogic.Entity.Actor.Model
{
    public interface IActorModel : IDieable
    {
        string Name { get; }
        ActorStatus Status { get; }
        Inventory Inventory { get; }
        Vector2Int Position { get; }
        ActorType ActorType { get; }
        IObservable<IActResult> OnActResult { get; }

        void SendResultMessage(IActResult result);
        void GetDamage(IActorModel dealer, AttackStatus attack);
        void Attack(IActorModel target);
        void StepTo(Vector2Int position);
        Vector2Int GetPositionWithDirection(ActorDirection direction);
        void WarpTo(Vector2Int position);
        void Heal(int amount);
    }
}