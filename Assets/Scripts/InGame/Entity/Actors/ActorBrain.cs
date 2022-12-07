using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace yumehiko.LOF
{
	/// <summary>
    /// Actorの挙動を決めるクラス。
    /// </summary>
	public abstract class ActorBrain : MonoBehaviour, IActor
	{
        public EntityType EntityType => EntityType.Actor;
        public abstract Affiliation Affiliation { get; }
        public abstract ActorStatus Status { get; }

        public abstract UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token);

        public abstract void SetProfile(ActorStatus status, ActorVisual visual);
    }
}