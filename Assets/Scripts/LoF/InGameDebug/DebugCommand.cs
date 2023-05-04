using System;
using LoF.GameLogic.Entity.Actor;
using LoF.Input;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace LoF.InGameDebug
{
    public class DebugCommand : IInitializable, IDisposable
    {
        private readonly Actors actors;
        private IDisposable disposable;

        [Inject]
        public DebugCommand(Actors actors)
        {
            this.actors = actors;
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }

        public void Initialize()
        {
            disposable = ReactiveInput.OnDebug
                .Where(isTrue => isTrue)
                .Subscribe(_ => actors.DefeatAllEnemy());
        }
    }
}