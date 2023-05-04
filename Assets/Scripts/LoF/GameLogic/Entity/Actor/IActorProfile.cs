using System.Collections.Generic;
using LoF.GameLogic.Entity.Actor.Brains;
using LoF.GameLogic.Entity.Item.Card;

namespace LoF.GameLogic.Entity.Actor
{
    public interface IActorProfile
    {
        BrainType BrainType { get; }
        ActorView View { get; }
        string ActorName { get; }
        int BaseHP { get; }
        CardProfile Weapon { get; }
        CardProfile Armor { get; }
        IReadOnlyList<CardProfile> InventoryCards { get; }
    }
}