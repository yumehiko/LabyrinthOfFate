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
        [SerializeField] private int ad;

        public int AD => ad;
        public int ID { get; }

        public AttackStatus(AttackStatus status, int id)
        {
            this.ad = status.ad;
            ID = id;
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