using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
    public interface ICard : IItem
    {
        string CardName { get; }
        Sprite Frame { get; }
        AttackStatuses AttackStatuses { get; }
        DefenceStatus DefenceStatus { get; }
        string InvokeEffect { get; }

        void SetCopyParameter(Card target);
        Card MakeCopy();
    }
}