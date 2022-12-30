using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Presenter
{
    [System.Serializable]
    public class PlayerProfile : MonoBehaviour, IActorProfile
    {
		public BrainType BrainType => brainType;
		public ActorView View => view;
		public string ActorName => actorName;
		public int BaseHP => baseHP;
		public CardProfile Weapon => weapon;
		public CardProfile Armor => armor;

		[SerializeField] private BrainType brainType;
		[SerializeField] private ActorView view;
		[SerializeField] private string actorName;
		[SerializeField] int baseHP;
		[SerializeField] private CardProfile weapon;
		[SerializeField] private CardProfile armor;
	}
}