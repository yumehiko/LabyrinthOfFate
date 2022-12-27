using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		string InvokeEffect { get; }
	}

	public enum ITemType
    {
		None,
		Card,
    }
}