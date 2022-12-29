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
        [SerializeField] private Transform viewParent;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(viewParent);
            builder.Register<Actors>(Lifetime.Singleton);
        }
    }
}