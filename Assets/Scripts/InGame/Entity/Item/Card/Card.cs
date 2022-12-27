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
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card")]
    public class Card : ScriptableObject, ICard
    {
        public ITemType Type => ITemType.Card;
        public string CardName => cardName;
        public Sprite Frame => frame;
        public AttackStatuses AttackStatuses => attackStatuses;
        public DefenceStatus DefenceStatus => defenceStatus;
        public string InvokeEffect => invokeEffect;

        public IObservable<Unit> OnRemove => onRemove;

        private readonly Subject<Unit> onRemove = new Subject<Unit>();

        [SerializeField] private string cardName;
        [SerializeField] private Sprite frame;
        [SerializeField] private AttackStatuses attackStatuses;
        [SerializeField] private DefenceStatus defenceStatus;
        [SerializeField] private string invokeEffect;

        /// <summary>
        /// 装備されているなら、これを外す。
        /// </summary>
        public void TryRemoveBySlot()
        {
            onRemove.OnNext(Unit.Default);
        }

        /// <summary>
        /// このカードに指定したカードの情報を全てディープコピーする。
        /// </summary>
        /// <param name="target"></param>
        public void SetCopyParameter(Card target)
        {
            cardName = target.CardName;
            frame = target.Frame;
            invokeEffect = target.InvokeEffect;
            attackStatuses = new AttackStatuses(target.AttackStatuses);
            defenceStatus = new DefenceStatus(target.DefenceStatus);
        }

        /// <summary>
        /// このカードのコピーを作る。
        /// </summary>
        /// <returns></returns>
        public Card MakeCopy()
        {
            var copy = CreateInstance<Card>();
            copy.SetCopyParameter(this);
            return copy;
        }
    }
}