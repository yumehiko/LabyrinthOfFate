using VContainer;
using VContainer.Unity;

namespace yumehiko
{

    public class DebugLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<DebugCommand>(Lifetime.Singleton);
        }
    }
}