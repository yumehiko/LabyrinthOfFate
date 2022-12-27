using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.View;
using yumehiko.LOF.Presenter;

namespace yumehiko.LOF.Model
{
    public interface IActorProfile
    {
        BrainType BrainType { get; }
        ActorView View { get; }
        string ActorName { get; }
        int BaseHP { get; }
        ICard Weapon { get; }
        ICard Armor { get; }
    }
}