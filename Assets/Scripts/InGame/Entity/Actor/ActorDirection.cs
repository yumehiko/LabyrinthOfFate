using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.LOF
{
    /// <summary>
    /// Actorの概念的な上下左右4方向を示す。
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
        DownLeft,
    }

    /// <summary>
    /// ActorDirectionのReactiveProperty。
    /// </summary>
    [System.Serializable]
    public class ActorDirectionReactiveProperty : ReactiveProperty<ActorDirection>
    {
        public ActorDirectionReactiveProperty() { }
        public ActorDirectionReactiveProperty(ActorDirection initialValue) : base(initialValue) { }
    }
}