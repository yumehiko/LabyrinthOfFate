using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
    [Serializable]
    public class DefenceStatus
    {
        public int HP => hp;
        [SerializeField] private int hp;

        public DefenceStatus(int hp)
        {
            this.hp = hp;
        }

        public DefenceStatus(DefenceStatus status)
        {
            this.hp = status.hp;
        }

        public string GetInfo()
        {
            const char heart = '♥';
            return $"{heart} {hp}";
        }

    }
}