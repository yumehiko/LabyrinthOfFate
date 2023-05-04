using System;
using UniRx;

namespace LoF.GameLogic.Entity.Actor.Model
{
    /// <summary>
    ///     Actorの概念的な8方向を示す。
    /// </summary>
    public enum ActorDirection
    {
        None,
        Up,
        Down,
        Right,
        Left,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }

    /// <summary>
    ///     ActorDirectionのReactiveProperty。
    /// </summary>
    [Serializable]
    public class ActorDirectionReactiveProperty : ReactiveProperty<ActorDirection>
    {
        public ActorDirectionReactiveProperty()
        {
        }

        public ActorDirectionReactiveProperty(ActorDirection initialValue) : base(initialValue)
        {
        }
    }
}