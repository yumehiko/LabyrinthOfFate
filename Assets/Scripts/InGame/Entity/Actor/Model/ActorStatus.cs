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
        public int Energy { get; private set; }
        public IReadOnlyReactiveProperty<int> MaxHP => maxHP;
        public IReadOnlyReactiveProperty<int> HP => hp;
        public IReadOnlyReactiveProperty<bool> IsDied => isDied;
        public EquipSlot EquipSlot { get; }
        public AttackStatuses AttackStatuses => EquipSlot.Weapon.AttackStatuses;
        public DefenceStatus DefenceStatus => EquipSlot.Armor.DefenceStatus;
        public Buffs Buffs { get; }

        private readonly IntReactiveProperty maxHP;
        private readonly IntReactiveProperty hp;
        private readonly int baseHP;
        private readonly BoolReactiveProperty isDied = new BoolReactiveProperty(false);
        private readonly IDisposable hpDisposable;

        public ActorStatus(IActorProfile profile, EquipSlot equipSlot)
        {
            Name = profile.ActorName;
            baseHP = profile.BaseHP;
            EquipSlot = equipSlot;
            maxHP = new IntReactiveProperty(baseHP + EquipSlot.AdditionalHP.Value);
            hp = new IntReactiveProperty(baseHP + EquipSlot.AdditionalHP.Value);
            Buffs = new Buffs();

            hpDisposable = EquipSlot.AdditionalHP.Subscribe(amount => ResetMaxHP(amount));
            Energy = 1;
        }

        public void Dispose()
        {
            hpDisposable.Dispose();
        }

        public void RefleshEnergy()
        {
            Energy = 1 + Buffs.AddibleEnergy;
        }

        public void UseEnergy()
        {
            Energy--;
        }

        public AttackStatus PickAttackStatus() => EquipSlot.PickRandomAttackStatus();

        /// <summary>
        /// ダメージを受ける。
        /// </summary>
        public void TakeDamage(Damage damage)
        {
            if (isDied.Value)
            {
                return;
            }

            //MEMO: なんか跳ね返したりする場合もあるし、攻撃者はメモっておきたいが現状は使わない。
            hp.Value = Mathf.Max(hp.Value - damage.Amount, 0);
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