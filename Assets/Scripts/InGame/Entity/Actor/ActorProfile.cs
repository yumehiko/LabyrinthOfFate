using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Presenter
{
	/// <summary>
	/// あるレベルに登場できるActorのプロフィール。
	/// </summary>
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ActorProfile")]
	public class ActorProfile : ScriptableObject, IActorProfile, ICardProfile
	{
		public BrainType BrainType => brainType;
		public ActorView View => view;
		public string ActorName => actorName;
		public int BaseHP => baseHP;
		public string CardName => actorName;
		public IReadOnlyList<AttackStatus> AttackStatuses => attackStatuses;
		public DefenceStatus DefenceStatus => defenceStatus;
		public ICardProfile Weapon => this;
		public ICardProfile Armor => this;
		public Sprite FrameSprite => frameSPrite;

		[SerializeField] private BrainType brainType;
		[SerializeField] private ActorView view;
		[SerializeField] private string actorName;
		[SerializeField] int baseHP;
		[Space(10)]
		[SerializeField] private List<AttackStatus> attackStatuses;
		[SerializeField] private DefenceStatus defenceStatus;
		[SerializeField] private Sprite frameSPrite;

		/// <summary>
        /// このプロファイルを元にカードを生成する。
        /// </summary>
        /// <returns></returns>
		public Card MakeCard()
		{
			List<AttackStatus> copyAttacks = new List<AttackStatus>();
			foreach (var attack in attackStatuses)
			{
				var copy = new AttackStatus(attack);
				copyAttacks.Add(copy);
			}
			DefenceStatus defence = new DefenceStatus(defenceStatus);
			var card = new Card(actorName, copyAttacks, defence);
			return card;
		}
	}
}