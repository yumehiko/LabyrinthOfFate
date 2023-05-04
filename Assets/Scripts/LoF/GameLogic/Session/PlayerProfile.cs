using System;
using System.Collections.Generic;
using LoF.GameLogic.Entity.Actor;
using LoF.GameLogic.Entity.Actor.Brains;
using LoF.GameLogic.Entity.Item.Card;
using UnityEngine;

namespace LoF.GameLogic.Session
{
    [Serializable]
    public class PlayerProfile : MonoBehaviour, IActorProfile
    {
        [SerializeField] private BrainType brainType;
        [SerializeField] private ActorView view;
        [SerializeField] private string actorName;
        [SerializeField] private int baseHP;
        [SerializeField] private CardProfile weapon;
        [SerializeField] private CardProfile armor;
        [SerializeField] private List<CardProfile> inventoryCards;
        public BrainType BrainType => brainType;
        public ActorView View => view;
        public string ActorName => actorName;
        public int BaseHP => baseHP;
        public CardProfile Weapon => weapon;
        public CardProfile Armor => armor;
        public IReadOnlyList<CardProfile> InventoryCards => inventoryCards;
    }
}