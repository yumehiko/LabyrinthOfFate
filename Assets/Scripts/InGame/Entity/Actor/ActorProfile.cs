using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Presenter;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Model
{
	/// <summary>
	/// あるレベルに登場できるActorのプロフィール。
	/// </summary>
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ActorProfile")]
	public class ActorProfile : ScriptableObject
	{
		public ActorView View => view;
		public ActorStatus Status => status;

		[SerializeField] private ActorView view;
		[SerializeField] private ActorStatus status;
	}
}