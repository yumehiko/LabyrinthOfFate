using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// あるActorの持つゲーム上のステータス。
    /// </summary>
	public class ActorStatus : IDisposable
    {
        public string Name { get; }
        public IReadOnlyReactiveProperty<int> MaxHP => maxHP;
        public IReadOnlyReactiveProperty<int> HP => hp;
        public IReadOnlyReactiveProperty<bool> IsDied => isDied;
        public EquipSlot EquipSlot => equipSlot;
        public AttackStatuses AttackStatuses => equipSlot.Weapon.AttackStatuses;
        public DefenceStatus DefenceStatus => equipSlot.Armor.DefenceStatus;

        private readonly IntReactiveProperty maxHP;
        private readonly IntReactiveProperty hp;
        private readonly int baseHP;
        private readonly EquipSlot equipSlot = new EquipSlot();
        private readonly BoolReactiveProperty isDied = new BoolReactiveProperty(false);
        private readonly IDisposable hpDisposable;

        public ActorStatus(IActorProfile profile)
        {
            Name = profile.ActorName;
            baseHP = profile.BaseHP;
            var weapon = profile.Weapon.MakeCopy();
            var armor = profile.Armor.MakeCopy();
            equipSlot.EquipWeapon(weapon);
            equipSlot.EquipArmor(armor);
            maxHP = new IntReactiveProperty(baseHP + equipSlot.AdditionalHP.Value);
            hp = new IntReactiveProperty(baseHP + equipSlot.AdditionalHP.Value);

            hpDisposable = equipSlot.AdditionalHP.Subscribe(amount => ResetMaxHP(amount));
        }

        public void Dispose()
        {
            hpDisposable.Dispose();
        }

        public AttackStatus PickAttackStatus() => equipSlot.PickRandomAttackStatus();

        /// <summary>
        /// ダメージを受ける。
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="ad"></param>
        public void GetDamage(IActor dealer, AttackStatus attackStatus)
        {
            if (isDied.Value)
            {
                return;
            }

            //MEMO: なんか跳ね返したりする場合もあるし、攻撃者はメモっておきたいが現状は使わない。
            hp.Value = Mathf.Max(hp.Value - attackStatus.AD, 0);
            if (hp.Value <= 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            hp.Value = Mathf.Min(hp.Value + amount, maxHP.Value);
        }

        public void Die()
        {
            isDied.Value = true;
        }

        private void ResetMaxHP(int amount)
        {
            maxHP.Value = baseHP + amount;
            hp.Value = Mathf.Min(hp.Value, maxHP.Value);
        }
    }
}