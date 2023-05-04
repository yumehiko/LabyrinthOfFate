using System.Threading;

namespace LoF.GameLogic.Entity.Actor
{
	/// <summary>
	///     ターンアクションの要求に必要な要素のまとまり。
	/// </summary>
	public class ActRequest
    {
        public ActRequest(float animationSpeedFactor, CancellationToken logicCT, CancellationToken animationCT)
        {
            SpeedFactor = animationSpeedFactor;
            LogicCT = logicCT;
            AnimationCT = animationCT;
        }

        public float SpeedFactor { get; }
        public CancellationToken LogicCT { get; }
        public CancellationToken AnimationCT { get; }
    }
}