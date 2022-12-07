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
        /// 所属。
        /// </summary>
		Affiliation Affiliation { get; }

		/// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        /// <param name="animationSpeedFactor"></param>
		UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token);
	}
}