using System;
using UnityEngine;

namespace LoF.GameLogic.Entity.Item.Card
{
    /// <summary>
    ///     アイテムの攻撃側が持つステータス。
    /// </summary>
    [Serializable]
    public class AttackStatus
    {
        [SerializeField] private int ad;

        public AttackStatus(AttackStatus status, int id)
        {
            ad = status.ad;
            ID = id;
        }

        public int AD => ad;
        public int ID { get; }

        /// <summary>
        ///     このステータスがミスに相当するかを返す。
        /// </summary>
        /// <returns></returns>
        public bool IsMiss()
        {
            return ad == 0;
        }

        /// <summary>
        ///     ステータス情報をstringで取得。
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            //TODO:属性ダメージとかもいる
            return $"\u2694{ad}";
        }
    }
}