using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Invoke;

namespace yumehiko.LOF.Model
{
	/// <summary>
    /// アイテム。
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
		Card,
    }
}