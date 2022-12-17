using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF.Model
{
	/// <summary>
    /// Actorの種類、陣営を示す。
    /// </summary>
	public enum ActorType
	{
		None,
		Player,  //プレイヤーの味方陣営みたいなのは（必要なら）、Playerとは別に定義する。
		Enemy,
	}
}