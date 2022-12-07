using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace yumehiko.LOF
{
	public interface IActor : IEntity
	{
		/// <summary>
        /// 陣営。
        /// </summary>
		Affiliation Affiliation { get; }

		/// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        /// <param name="animationSpeedFactor"></param>
		UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token);

		/// <summary>
        /// StatusやVisualをセットする。
        /// </summary>
        /// <param name="profile"></param>
		void SetProfile(ActorStatus status, ActorVisual visual);

		/// <summary>
        /// このActorのステータス。
        /// </summary>
		ActorStatus Status { get; }
	}
}