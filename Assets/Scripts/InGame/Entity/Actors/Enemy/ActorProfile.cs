using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
	/// <summary>
    /// あるレベルに登場できるActorのプロフィール。
    /// </summary>
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ActorProfile")]
	public class ActorProfile : ScriptableObject
	{
		public ActorBrain Prefab => prefab;
		public string Name => actorName;
		public int Hp => hp;
		public int AttackDamage => attackDamage;

		[SerializeField] private ActorBrain prefab;
		[SerializeField] private string actorName;
		[SerializeField] private int hp;
		[SerializeField] private int attackDamage;
	}
}