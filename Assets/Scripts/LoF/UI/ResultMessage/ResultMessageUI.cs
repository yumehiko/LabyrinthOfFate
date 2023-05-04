using System;
using LoF.GameLogic.Entity.Actor;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace LoF.UI.ResultMessage
{
    public class ResultMessageUI : IDisposable, IInitializable
    {
        private readonly Actors actors;
        private readonly ResultMessageUIView view;
        private CompositeDisposable actorMessageDisposable;
        private IDisposable dispose;

        [Inject]
        public ResultMessageUI(Actors actors, ResultMessageUIView view)
        {
            this.actors = actors;
            this.view = view;
        }

        public void Dispose()
        {
            dispose?.Dispose();
            actorMessageDisposable?.Dispose();
        }

        public void Initialize()
        {
            view.ViewMessage("You Enter the Dungeon");
            dispose = actors.OnSetLevelActors.Subscribe(_ => SubscribeActorMessages(actors));
        }

        public void SubscribeActorMessages(Actors actors)
        {
            actorMessageDisposable?.Dispose();
            actorMessageDisposable = new CompositeDisposable();

            actors.Player.Model.OnActResult
                .Subscribe(result => view.ViewMessage(result.GetMessage()))
                .AddTo(actorMessageDisposable);

            foreach (var enemy in actors.Enemies)
                enemy.Model.OnActResult
                    .Subscribe(result => view.ViewMessage(result.GetMessage()))
                    .AddTo(actorMessageDisposable);
        }
    }
}