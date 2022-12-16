using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.LOF.View
{
	public interface IActorView
	{
		void DestroySelf();
		UniTask WaitAnimation(float animationSpeedFactor, CancellationToken token);
		UniTask StepAnimation(float animationSpeedFactor, CancellationToken token);
		UniTask AttackAnimation(float animationSpeedFactor, CancellationToken token);
		UniTask ItemAnimation(float animationSpeedFactor, CancellationToken token);
	}
}