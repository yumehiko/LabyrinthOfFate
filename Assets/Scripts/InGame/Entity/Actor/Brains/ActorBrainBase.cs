using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Presenter
{
    public abstract class ActorBrainBase : IActorBrain
    {
        //TODO:モデルやViewを直接公開せず、presenter側でなんとかしてやりたいな。
        public abstract IActorModel Model { get; }
        public abstract IActorView View { get; }
        public bool HasEnergy => Model.Status.Energy > 0;
        public void RefleshEnergy() => Model.Status.RefleshEnergy();
        public abstract UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token);

        public virtual void Heal(int amount) => Model.Heal(amount);
        public virtual void WarpTo(Vector2Int position)
        {
            Model.WarpTo(position);
            View.WarpTo(position);
        }

        public virtual void Destroy()
        {
            View.DestroySelf();
        }
    }
}