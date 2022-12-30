using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using yumehiko.LOF.Invoke;


namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムのうち、装備や発動が可能なもの。
    /// このゲームにおけるほとんど全てのアイテム。
    /// TODO:これカードのProfileも兼用しちゃってるけど、分割した方が混同せずにすむ
    /// </summary>
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardProfile")]
    public class CardProfile : ScriptableObject
    {
        public ITemType Type => ITemType.Card;
        public string Name => cardName;
        public Sprite Frame => frame;
        public InvokeType InvokeType => invokeType;
        public AttackStatuses AttackStatuses => attackStatuses;
        public DefenceStatus DefenceStatus => defenceStatus;

        [SerializeField] private string cardName;
        [SerializeField] private Sprite frame;
        [SerializeField] private InvokeType invokeType;
        [SerializeField] private AttackStatuses attackStatuses;
        [SerializeField] private DefenceStatus defenceStatus;
    }
}