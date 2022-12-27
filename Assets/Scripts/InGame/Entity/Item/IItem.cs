using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
	/// <summary>
    /// アイテム。
    /// </summary>
	public interface IItem 
	{
		ITemType Type { get; }
	}

	public enum ITemType
    {
		None,
		Card,
    }
}