using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
    public interface ICardModel : IItemModel
    {
        AttackStatuses AttackStatuses { get; }
        DefenceStatus DefenceStatus { get; }

        void SetCopyParameter(CardModel target);
        CardModel MakeCopy();
    }
}