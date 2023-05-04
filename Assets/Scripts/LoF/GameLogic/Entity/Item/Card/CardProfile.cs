using LoF.GameLogic.Entity.Item.InvokeEffect;
using UnityEngine;

namespace LoF.GameLogic.Entity.Item.Card
{
    /// <summary>
    ///     アイテムのうち、装備や発動が可能なもの。
    ///     このゲームにおけるほとんど全てのアイテム。
    ///     TODO:これカードのProfileも兼用しちゃってるけど、分割した方が混同せずにすむ
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardProfile")]
    public class CardProfile : ScriptableObject
    {
        [SerializeField] private string cardName;
        [SerializeField] private Sprite frame;
        [SerializeField] private InvokeType invokeType;
        [SerializeField] private AttackStatuses attackStatuses;
        [SerializeField] private DefenceStatus defenceStatus;
        public ITemType Type => ITemType.Card;
        public string Name => cardName;
        public Sprite Frame => frame;
        public InvokeType InvokeType => invokeType;
        public AttackStatuses AttackStatuses => attackStatuses;
        public DefenceStatus DefenceStatus => defenceStatus;
    }
}