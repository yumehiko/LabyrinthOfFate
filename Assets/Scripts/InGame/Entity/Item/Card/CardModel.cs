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
    public class CardModel : ScriptableObject, ICardModel
    {
        public ITemType Type => ITemType.Card;
        public string Name => cardName;
        public Sprite Frame => frame;
        public string InvokeEffect => invokeEffect;
        public string StatsInfo => GetInfo();
        public AttackStatuses AttackStatuses => attackStatuses;
        public DefenceStatus DefenceStatus => defenceStatus;

        public IObservable<Unit> OnRemove => onRemove;

        private readonly Subject<Unit> onRemove = new Subject<Unit>();

        [SerializeField] private string cardName;
        [SerializeField] private Sprite frame;
        [SerializeField] private string invokeEffect;
        [SerializeField] private AttackStatuses attackStatuses;
        [SerializeField] private DefenceStatus defenceStatus;

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
        public void SetCopyParameter(CardModel target)
        {
            cardName = target.Name;
            frame = target.Frame;
            invokeEffect = target.InvokeEffect;
            attackStatuses = new AttackStatuses(target.AttackStatuses);
            defenceStatus = new DefenceStatus(target.DefenceStatus);
        }

        /// <summary>
        /// このカードのコピーを作る。
        /// </summary>
        /// <returns></returns>
        public CardModel MakeCopy()
        {
            var copy = CreateInstance<CardModel>();
            copy.SetCopyParameter(this);
            return copy;
        }

        private string GetInfo()
        {
            var stats = AttackStatuses.GetInfo();
            stats += Environment.NewLine;
            stats += DefenceStatus.GetInfo();
            return stats;
        }
    }
}