using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace yumehiko.LOF
{
	/// <summary>
    /// Actorの挙動を決めるクラス。
    /// TODO:物理判定（GetComponent）から取得するのがこいつになってるが、Bodyを取得してBrainはBodyにくっついてるという関係の方がよさそう。
    /// </summary>
	public abstract class ActorBrain : MonoBehaviour, IActor
	{
        public EntityType EntityType => EntityType.Actor;
        public abstract Affiliation Affiliation { get; }
        public ActorBody Body => body;

        protected ActorVisual visual;
        protected ActorBody body;

        public abstract UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token);

        public void SetProfile(ActorStatus status, ActorVisual visual)
        {
            if (body != null)
            {
                return;
            }
            body = new ActorBody(status);
            this.visual = visual;
        }
    }
}