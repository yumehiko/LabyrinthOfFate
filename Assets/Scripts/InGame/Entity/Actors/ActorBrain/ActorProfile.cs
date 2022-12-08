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
		public ActorBrain Brain => brain;
		public ActorVisual Visual => visual;
		public ActorStatus Status => status;

		[SerializeField] private ActorBrain brain;
		[SerializeField] private ActorVisual visual;
		[SerializeField] private ActorStatus status;
	}
}