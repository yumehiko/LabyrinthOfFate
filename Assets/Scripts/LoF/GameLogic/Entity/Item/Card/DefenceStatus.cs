using System;
using UnityEngine;

namespace LoF.GameLogic.Entity.Item.Card
{
    [Serializable]
    public class DefenceStatus
    {
        [SerializeField] private int hp;

        public DefenceStatus(int hp)
        {
            this.hp = hp;
        }

        public DefenceStatus(DefenceStatus status)
        {
            hp = status.hp;
        }

        public int HP => hp;

        public string GetInfo()
        {
            const char heart = '♥';
            return $"{heart} {hp}";
        }
    }
}