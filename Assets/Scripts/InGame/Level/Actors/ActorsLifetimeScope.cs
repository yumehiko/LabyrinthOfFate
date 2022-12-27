using VContainer;
using VContainer.Unity;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using yumehiko.LOF.Presenter;

namespace yumehiko.LOF
{
    public class ActorsLifetimeScope : LifetimeScope
    {
        [SerializeField] private ActorViews actorViews;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(actorViews);
            builder.Register<ActorModels>(Lifetime.Singleton);
            builder.Register<ActorPresenters>(Lifetime.Singleton);
        }
    }
}