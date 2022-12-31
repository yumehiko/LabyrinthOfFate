using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Presenter;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.Invoke
{
	/// <summary>
    /// アイテムの発動効果を表す。
    /// </summary>
	public interface IInvokeEffect
	{
		InvokeType Type { get; }
		string Info { get; }
		void Invoke(IActorModel user, IItemModel parent);
	}
}