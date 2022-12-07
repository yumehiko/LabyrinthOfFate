using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
	/// <summary>
    /// Actorの陣営を示す。
    /// </summary>
	public enum Affiliation 
	{
		Player, //プレイヤー専用の陣営。
		Enemy, //敵陣営。
		//これ以外に、プレイヤー側陣営なども追加できるだろう。
	}
}