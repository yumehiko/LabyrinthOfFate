using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
	public class BattleResult :IActResult
	{
        private readonly IActorModel dealer;
        private readonly IActorModel taker;
        private readonly Damage damage;

        public BattleResult(IActorModel dealer, IActorModel taker, Damage damage)
        {
            this.dealer = dealer;
            this.taker = taker;
            this.damage = damage;
        }

        public string GetMessage()
        {
            if (damage.Amount == 0)
            {
                return $"{damage.Dice}  MISS!";
            }
            var dealerName = dealer.ActorType == ActorType.Player ? "You" : dealer.Name;
            var takerName = taker.ActorType == ActorType.Player ? "You" : taker.Name;

            return $"{damage.Dice}  {dealerName} hits {takerName} for {damage.Amount} damage!";
        }
    }
}