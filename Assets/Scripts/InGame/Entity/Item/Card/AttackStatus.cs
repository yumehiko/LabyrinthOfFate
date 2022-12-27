using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// アイテムの攻撃側が持つステータス。
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

        /// <summary>
        /// このステータスがミスに相当するかを返す。
        /// </summary>
        /// <returns></returns>
        public bool IsMiss()
        {
            return ad == 0;
        }

        /// <summary>
        /// ステータス情報をstringで取得。
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            //TODO:属性ダメージとかもいる
            return $"\u2694{ad}";
        }
    }
}