using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// 1度の攻撃による結果を示す。
    /// </summary>
    public class Damage
    {
        public static readonly string diceChars = "\u2680\u2681\u2682\u2683\u2684\u2685";

        public char Dice { get; }
        public int Amount { get; }

        public Damage(AttackStatus attack, DefenceStatus defence)
        {
            Dice = diceChars[attack.ID];
            Amount = attack.AD;
            //TODO:属性攻撃や耐性の処理もいずれ入れる。
        }
    }
}