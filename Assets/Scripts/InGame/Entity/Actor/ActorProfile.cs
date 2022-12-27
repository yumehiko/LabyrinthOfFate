using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// あるレベルに登場できるActorのプロフィール。
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ActorProfile")]
    public class ActorProfile : ScriptableObject, IActorProfile
    {
        public BrainType BrainType => brainType;
        public ActorView View => view;
        public string ActorName => card.Name;
        public int BaseHP => baseHP;

        public ICardModel Weapon => card;
        public ICardModel Armor => card;

        [SerializeField] private BrainType brainType;
        [SerializeField] private ActorView view;
        [SerializeField] int baseHP;
        [Space(10)]
        [SerializeField] private CardModel card;
    }
}