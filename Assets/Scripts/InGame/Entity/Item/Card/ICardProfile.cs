using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
    public interface ICardProfile
    {
        string CardName { get; }
        IReadOnlyList<AttackStatus> AttackStatuses { get; }
        DefenceStatus DefenceStatus { get; }
        Card MakeCard();
    }
}