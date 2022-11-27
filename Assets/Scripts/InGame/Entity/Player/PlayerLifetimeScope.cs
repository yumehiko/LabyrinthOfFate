using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace yumehiko.LOF
{
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField] private ActorView view;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PlayerInput>();
            builder.Register<GridMovement>(Lifetime.Singleton);
            builder.RegisterComponent(view);
        }
    }
}