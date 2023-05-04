using System.Collections.Generic;
using LoF.GameLogic.Entity.Actor.Brains;
using LoF.GameLogic.Entity.Item.Card;
using UnityEngine;

namespace LoF.GameLogic.Entity.Actor
{
    /// <summary>
    ///     あるレベルに登場できるActorのプロフィール。
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ActorProfile")]
    public class ActorProfile : ScriptableObject, IActorProfile
    {
        [SerializeField] private BrainType brainType;
        [SerializeField] private ActorView view;
        [SerializeField] private int baseHP;

        [Space(10)] [SerializeField] private CardProfile card;

        [SerializeField] private List<CardProfile> inventoryCards;
        public BrainType BrainType => brainType;
        public ActorView View => view;
        public string ActorName => card.Name;
        public int BaseHP => baseHP;
        public CardProfile Weapon => card;
        public CardProfile Armor => card;
        public IReadOnlyList<CardProfile> InventoryCards => inventoryCards;
    }
}