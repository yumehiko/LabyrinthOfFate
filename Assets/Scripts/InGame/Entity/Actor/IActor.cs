using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace yumehiko.LOF.Model
{
	public interface IActor : IDieable
	{
		string Name { get; }
		ActorStatus Status { get; }
		Vector2Int Position { get; }
		ActorType ActorType { get; }

		void GetDamage(IActor dealer, AttackStatus attack);
		void Attack(IActor target);
		void StepTo(Vector2Int position);
		Vector2Int GetPositionWithDirection(ActorDirection direction);
		void WarpTo(Vector2Int position);
	}
}