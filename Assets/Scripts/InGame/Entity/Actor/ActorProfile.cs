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
        public string ActorName => card.CardName;
        public int BaseHP => baseHP;

        public ICard Weapon => card;
        public ICard Armor => card;

        [SerializeField] private BrainType brainType;
        [SerializeField] private ActorView view;
        [SerializeField] int baseHP;
        [Space(10)]
        [SerializeField] private Card card;
    }
}