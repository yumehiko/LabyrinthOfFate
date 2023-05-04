using LoF.GameLogic.Entity.Item.InvokeEffect;
using UnityEngine;

namespace LoF.GameLogic.Entity.Item
{
	/// <summary>
	///     アイテム。
	/// </summary>
	public interface IItemModel
    {
        ITemType Type { get; }
        string Name { get; }
        Sprite Frame { get; }
        string StatsInfo { get; }
        IInvokeEffect InvokeEffect { get; }
    }

    public enum ITemType
    {
        None,
        Card
    }
}