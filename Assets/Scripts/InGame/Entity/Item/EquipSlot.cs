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
            weapon = card;
        }

        public void EquipArmor(Card card)
        {
            armor = card;
        }

        public AttackStatus PickRandomAttackStatus()
        {
            var attack = weapon.AttackStatuses.PickRandomAttack();
            return attack;
        }
    }
}