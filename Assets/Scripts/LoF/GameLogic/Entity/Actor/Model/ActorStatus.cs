using System;
using LoF.GameLogic.Entity.Buff;
using LoF.GameLogic.Entity.Item;
using LoF.GameLogic.Entity.Item.Card;
using UniRx;
using UnityEngine;

namespace LoF.GameLogic.Entity.Actor.Model
{
    /// <summary>
    ///     あるActorの持つゲーム上のステータス。
    /// </summary>
    public class ActorStatus : IDisposable
    {
        private readonly int baseHP;
        private readonly IntReactiveProperty hp;
        private readonly IDisposable hpDisposable;
        private readonly BoolReactiveProperty isDied = new(false);

        private readonly IntReactiveProperty maxHP;

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

        public string Name { get; }
        public int Energy { get; private set; }
        public IReadOnlyReactiveProperty<int> MaxHP => maxHP;
        public IReadOnlyReactiveProperty<int> HP => hp;
        public IReadOnlyReactiveProperty<bool> IsDied => isDied;
        public EquipSlot EquipSlot { get; }
        public AttackStatuses AttackStatuses => EquipSlot.Weapon.AttackStatuses;
        public DefenceStatus DefenceStatus => EquipSlot.Armor.DefenceStatus;
        public Buffs Buffs { get; }

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

        public AttackStatus PickAttackStatus()
        {
            return EquipSlot.PickRandomAttackStatus();
        }

        /// <summary>
        ///     ダメージを受ける。
        /// </summary>
        public void TakeDamage(Damage damage)
        {
            if (isDied.Value) return;

            //MEMO: なんか跳ね返したりする場合もあるし、攻撃者はメモっておきたいが現状は使わない。
            hp.Value = Mathf.Max(hp.Value - damage.Amount, 0);
            if (hp.Value <= 0) Die();
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