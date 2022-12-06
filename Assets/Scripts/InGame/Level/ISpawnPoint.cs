using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
	public interface ISpawnPoint
	{
		Affiliation Affiliation { get; }
		Vector2 Position { get; }
	}
}