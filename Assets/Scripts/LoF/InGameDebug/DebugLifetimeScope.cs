using VContainer;
using VContainer.Unity;

namespace LoF.InGameDebug
{
    public class DebugLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<DebugCommand>();
        }
    }
}