using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
	public class ActorStatus
	{
        public string Name { get; private set; }
        public int Hp { get; private set; }
        public int AttackDamage { get; private set; }

        public void SetStatus(ActorProfile profile)
        {
            Name = profile.Name;
            Hp = profile.Hp;
            AttackDamage = profile.AttackDamage;
        }
    }
}