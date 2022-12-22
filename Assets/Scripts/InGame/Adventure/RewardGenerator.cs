using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// レベルクリア時の報酬を生成する。
    /// </summary>
    public class RewardGenerator : MonoBehaviour
    {
        [SerializeField] private List<CardProfile> candidateCards;

        public CardProfile PickCandiateCard()
        {
            var id = Random.Range(0, candidateCards.Count);
            return candidateCards[id];
        }
    }
}