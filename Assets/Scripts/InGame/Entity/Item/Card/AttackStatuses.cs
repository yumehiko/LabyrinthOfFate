using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
	/// <summary>
    /// AttackStatusのコレクション。
    /// </summary>
    [Serializable]
	public class AttackStatuses
	{
        [SerializeField] private List<AttackStatus> list = new List<AttackStatus>();

        public AttackStatuses(AttackStatuses attackStatuses)
        {
            for(int i = 0; i < 6; i++)
            {
                var copy = new AttackStatus(attackStatuses.list[i], i);
                list.Add(copy);
            }
        }

        public AttackStatus PickRandomAttack()
        {
            int id = UnityEngine.Random.Range(0, list.Count);
            return list[id];
        }

        public string GetInfo()
        {
            const string diceChars = "\u2680\u2681\u2682\u2683\u2684\u2685";
            var info = "";
            for (int i = 0; i < 6; i++)
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