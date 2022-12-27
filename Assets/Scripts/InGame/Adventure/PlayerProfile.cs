using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Model
{
    [System.Serializable]
    public class PlayerProfile : MonoBehaviour, IActorProfile
    {
		public BrainType BrainType => brainType;
		public ActorView View => view;
		public string ActorName => actorName;
		public int BaseHP => baseHP;
		public ICard Weapon => weapon;
		public ICard Armor => armor;

		[SerializeField] private BrainType brainType;
		[SerializeField] private ActorView view;
		[SerializeField] private string actorName;
		[SerializeField] int baseHP;
		[SerializeField] private Card weapon;
		[SerializeField] private Card armor;
	}
}