using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Model
{
	[System.Serializable]
	public class PlayerInformation
	{
		[SerializeField] private ActorView view;
		[SerializeField] private ActorStatus status;

		public ActorView View => view;
		public ActorStatus Status => status;
	}
}