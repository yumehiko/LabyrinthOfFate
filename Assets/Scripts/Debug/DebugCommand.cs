using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using yumehiko.Input;
using yumehiko.LOF.Presenter;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using VContainer;
using VContainer.Unity;

namespace yumehiko
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