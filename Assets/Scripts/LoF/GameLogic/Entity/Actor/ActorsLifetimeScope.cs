using LoF.Effect;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LoF.GameLogic.Entity.Actor
{
    public class ActorsLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform viewParent;
        [SerializeField] private EffectController effectController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(viewParent);
            builder.RegisterComponent(effectController);
            builder.Register<Actors>(Lifetime.Singleton);
        }
    }
}