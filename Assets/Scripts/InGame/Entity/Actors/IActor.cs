using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        /// <param name="timeFactor"></param>
		UniTask DoTurnAction(float timeFactor);
	}
}