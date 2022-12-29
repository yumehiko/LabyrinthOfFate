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
        public ICardModel Weapon => weapon;
        public ICardModel Armor => armor;
        public IReadOnlyReactiveProperty<int> AdditionalHP => additionalHP;

        private readonly IntReactiveProperty additionalHP = new IntReactiveProperty();
        private ICardModel weapon;
        private ICardModel armor;

        public EquipSlot(ICardModel weapon, ICardModel armor)
        {
            EquipWeapon(weapon);
            EquipArmor(armor);
        }

        public void EquipWeapon(ICardModel card)
        {
            weapon = card;
        }

        public void EquipArmor(ICardModel card)
        {
            armor = card;
            additionalHP.Value = armor.DefenceStatus.HP;
        }

        public AttackStatus PickRandomAttackStatus()
        {
            var attack = weapon.AttackStatuses.PickRandomAttack();
            return attack;
        }
    }
}