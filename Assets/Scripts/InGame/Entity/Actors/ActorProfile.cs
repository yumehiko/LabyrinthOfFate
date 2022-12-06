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
		[SerializeField] private ActorBrain prefab;

		/// <summary>
		/// このプロフィールが指定するActorBrainプレハブ。
		/// </summary>
		public ActorBrain Prefab => prefab;
	}
}