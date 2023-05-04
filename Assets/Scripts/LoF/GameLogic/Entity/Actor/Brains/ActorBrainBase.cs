using Cysharp.Threading.Tasks;
using LoF.GameLogic.Entity.Actor.Model;
using UnityEngine;

namespace LoF.GameLogic.Entity.Actor.Brains
{
    public abstract class ActorBrainBase : IActorBrain
    {
        //TODO:モデルやViewを直接公開せず、presenter側でなんとかしてやりたいな。
        public abstract IActorModel Model { get; }
        public abstract IActorView View { get; }
        public bool HasEnergy => Model.Status.Energy > 0;

        public void RefleshEnergy()
        {
            Model.Status.RefleshEnergy();
        }

        public abstract UniTask DoTurnAction(ActRequest request);

        public virtual void Heal(int amount)
        {
            Model.Heal(amount);
        }

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