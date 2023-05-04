using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LoF.GameLogic.Entity.Item.Card
{
    /// <summary>
    ///     AttackStatusのコレクション。
    /// </summary>
    [Serializable]
    public class AttackStatuses
    {
        [SerializeField] private List<AttackStatus> list = new();

        public AttackStatuses(AttackStatuses attackStatuses)
        {
            for (var i = 0; i < 6; i++)
            {
                var copy = new AttackStatus(attackStatuses.list[i], i);
                list.Add(copy);
            }
        }

        public AttackStatus PickRandomAttack()
        {
            var id = Random.Range(0, list.Count);
            return list[id];
        }

        public string GetInfo()
        {
            const string diceChars = "\u2680\u2681\u2682\u2683\u2684\u2685";
            var info = "";
            for (var i = 0; i < 6; i++)
            {
                info += i != 0 ? Environment.NewLine : string.Empty; //1行目以外はまず改行を追加
                if (list[i].IsMiss())
                {
                    info += $"{diceChars[i]} MISS";
                    continue;
                }

                //属性攻撃とかあったらたす。
                info += $"{diceChars[i]} {list[i].GetInfo()}";
            }

            return info;
        }
    }
}