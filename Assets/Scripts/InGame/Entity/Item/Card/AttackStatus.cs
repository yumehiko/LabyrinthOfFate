using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムの攻撃側が持つ性能。
    /// </summary>
    [Serializable]
    public class AttackStatus
    {
        public int AD => ad;
        [SerializeField] private int ad;

        public AttackStatus(int ad)
        {
            this.ad = ad;
        }

        public AttackStatus(AttackStatus status)
        {
            this.ad = status.ad;
        }
    }
}