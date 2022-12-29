using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

namespace yumehiko.LOF.Model
{
	public interface IActorModel : IDieable
	{
		string Name { get; }
		ActorStatus Status { get; }
		Vector2Int Position { get; }
		ActorType ActorType { get; }
		IObservable<IActResult> OnActResult { get; }

		void GetDamage(IActorModel dealer, AttackStatus attack);
		void Attack(IActorModel target);
		void StepTo(Vector2Int position);
		Vector2Int GetPositionWithDirection(ActorDirection direction);
		void WarpTo(Vector2Int position);
		void Heal(int amount);
	}
}