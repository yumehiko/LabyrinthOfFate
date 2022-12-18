using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムのうち、装備や発動が可能なもの。
    /// このゲームにおけるほとんど全てのアイテム。
    /// </summary>
    [Serializable]
    public class Card : IItem
    {
        public string CardName => cardName;
        public IReadOnlyList<AttackStatus> AttackStatuses => attackStatuses;
        public DefenceStatus DefenceStatus => defenceStatus;
        public IObservable<Unit> OnRemove => onRemove;

        private readonly Subject<Unit> onRemove = new Subject<Unit>();

        [SerializeField] private string cardName;
        [SerializeField] private List<AttackStatus> attackStatuses;
        [SerializeField] private DefenceStatus defenceStatus;

        public Card(string cardName, List<AttackStatus> attackStatuses, DefenceStatus defenceStatus)
        {
            this.cardName = cardName;
            this.attackStatuses = attackStatuses;
            this.defenceStatus = defenceStatus;
        }

        /// <summary>
        /// 装備されているなら、これを外す。
        /// </summary>
        public void TryRemoveBySlot()
        {
            onRemove.OnNext(Unit.Default);
        }

        public static Card GetBareHand()
        {
            var attackStatuses = new List<AttackStatus>();
            for (int i = 0; i < 6; i++)
            {
                var bareHandAttack = new AttackStatus(1);
                attackStatuses.Add(bareHandAttack);
            }
            var defenceStatus = new DefenceStatus(0);
            var card = new Card("BareHand", attackStatuses, defenceStatus);
            return card;
        }
    }
}