using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace yumehiko.LOF
{
	/// <summary>
    /// Actorの挙動を決めるクラス。
    /// </summary>
	public class ActorBrain : MonoBehaviour, IActor
	{
        public EntityType EntityType => EntityType.Actor;
		public virtual Affiliation Affiliation { get; }

        public virtual UniTask DoTurnAction(float timeFactor)
        {
            throw new System.NotImplementedException();
        }
    }
}