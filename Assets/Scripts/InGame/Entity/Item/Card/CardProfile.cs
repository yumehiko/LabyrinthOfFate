using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardProfile")]
	public class CardProfile : ScriptableObject, ICardProfile
	{
        public string CardName => cardName;
        public IReadOnlyList<AttackStatus> AttackStatuses => attackStatuses;
        public DefenceStatus DefenceStatus => defenceStatus;
        public Sprite FrameSprite => frameSprite;

        [SerializeField] private string cardName;
        [SerializeField] private List<AttackStatus> attackStatuses;
        [SerializeField] private DefenceStatus defenceStatus;
        [SerializeField] private Sprite frameSprite;

        /// <summary>
        /// このプロファイルを元にカードを生成する。
        /// </summary>
        /// <returns></returns>
        public Card MakeCard()
        {
            List<AttackStatus> copyAttacks = new List<AttackStatus>();
            foreach (var attack in attackStatuses)
            {
                var copy = new AttackStatus(attack);
                copyAttacks.Add(copy);
            }
            DefenceStatus defence = new DefenceStatus(defenceStatus);
            var card = new Card(cardName, copyAttacks, defence);
            return card;
        }
    }
}