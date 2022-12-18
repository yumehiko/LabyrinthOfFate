using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace yumehiko.LOF.Model
{
	/// <summary>
	/// 武器用の装備スロット。
	/// </summary>
	public class EquipSlot
	{
		public Card Weapon => weapon;
		public Card Armor => armor;
		public int AdditionalHP => armor.DefenceStatus.HP;

		private Card weapon;
		private Card armor;

		public void EquipWeapon(Card card)
		{
			if(armor == card)
            {
				armor = Card.GetBareHand();
            }
			weapon = card;
		}

		public void EquipArmor(Card card)
		{
			if (weapon == card)
			{
				weapon = Card.GetBareHand();
			}
			armor = card;
		}

		public void RemoveWeapon()
        {
			weapon = Card.GetBareHand();
        }

		public void RemoveArmor()
        {
			armor = Card.GetBareHand();
        }

		public AttackStatus PickRandomAttackStatus()
		{
			int id = UnityEngine.Random.Range(0, weapon.AttackStatuses.Count);
			return weapon.AttackStatuses[id];
		}
	}
}