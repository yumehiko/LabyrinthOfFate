using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace yumehiko.LOF
{
	/// <summary>
    /// ターンアクションの要求に必要な要素のまとまり。
    /// </summary>
	public class ActRequest
	{
		public float SpeedFactor { get; }
		public CancellationToken LogicCT { get; }
		public CancellationToken AnimationCT { get; }

		public ActRequest(float animationSpeedFactor, CancellationToken logicCT, CancellationToken animationCT)
        {
			SpeedFactor = animationSpeedFactor;
			LogicCT = logicCT;
			AnimationCT = animationCT;
        }
	}
}