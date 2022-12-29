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
                return "MISS!";
            }
            return $"{damage.Dice}  {dealer.Name} hits {taker.Name} for {damage.Amount} damage!";
        }
    }
}