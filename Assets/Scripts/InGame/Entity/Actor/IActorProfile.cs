using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Presenter;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Model
{
    public interface IActorProfile
    {
        BrainType BrainType { get; }
        ActorView View { get; }
        string ActorName { get; }
        int BaseHP { get; }
        ICardProfile Weapon { get; }
        ICardProfile Armor { get; }
    }
}