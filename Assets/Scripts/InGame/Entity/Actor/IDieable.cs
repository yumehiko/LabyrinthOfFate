using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace yumehiko.LOF
{
	public interface IDieable
	{
		IReadOnlyReactiveProperty<bool> IsDied { get; }

		void Die();
	}
}