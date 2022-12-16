using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace yumehiko.LOF.Model
{
	public interface IActor : IEntity, IDieable
	{
		Vector2Int Position { get; }

		/// <summary>
        /// 陣営。
        /// </summary>
		ActorType ActorType { get; }

		/// <summary>
        /// ダメージを受ける。
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="ad"></param>
		void GetDamage(IActor dealer, int ad);
	}
}