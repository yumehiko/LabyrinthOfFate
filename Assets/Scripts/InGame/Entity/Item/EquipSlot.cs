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
        public CardModel Weapon => weapon;
        public CardModel Armor => armor;
        public int AdditionalHP => armor.DefenceStatus.HP;

        private CardModel weapon;
        private CardModel armor;

        public void EquipWeapon(CardModel card)
        {
            weapon = card;
        }

        public void EquipArmor(CardModel card)
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