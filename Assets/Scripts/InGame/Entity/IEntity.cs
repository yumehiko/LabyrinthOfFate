using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
	/// <summary>
    /// このゲームにおける、実体を伴うあらゆるもの。
    /// </summary>
	public interface IEntity
	{
		/// <summary>
        /// このエンティティの種類。
        /// </summary>
		EntityType EntityType { get; }
	}
}