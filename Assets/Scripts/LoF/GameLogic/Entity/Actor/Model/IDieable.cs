using UniRx;

namespace LoF.GameLogic.Entity.Actor.Model
{
    public interface IDieable
    {
        IReadOnlyReactiveProperty<bool> IsDied { get; }

        void Die();
    }
}