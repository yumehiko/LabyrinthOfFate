using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using UniRx;
using System;

namespace yumehiko.LOF.Presenter
{
    public class ResultMessageUI : IDisposable, IInitializable
    {
        private readonly ActorPresenters actors;
        private readonly ResultMessageUIView view;
        private IDisposable dispose;
        private CompositeDisposable actorMessageDisposable;

        [Inject]
        public ResultMessageUI(ActorPresenters actors, ResultMessageUIView view)
        {
            this.actors = actors;
            this.view = view;
        }

        public void Initialize()
        {
            view.ViewMessage("You Enter the Dungeon");
            dispose = actors.OnSetLevelActors.Subscribe(_ => SubscribeActorMessages(actors));
        }

        public void Dispose()
        {
            dispose?.Dispose();
            actorMessageDisposable?.Dispose();
        }

        public void SubscribeActorMessages(ActorPresenters actors)
        {
            actorMessageDisposable?.Dispose();
            actorMessageDisposable = new CompositeDisposable();

            actors.Player.Model.OnActResult
                .Subscribe(result => view.ViewMessage(result.GetMessage()))
                .AddTo(actorMessageDisposable);

            foreach (var enemy in actors.Enemies)
            {
                enemy.Model.OnActResult
                    .Subscribe(result => view.ViewMessage(result.GetMessage()))
                    .AddTo(actorMessageDisposable);
            }
        }
    }
}