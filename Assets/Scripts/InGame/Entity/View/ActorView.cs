using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using VContainer;

namespace yumehiko.LOF
{
	/// <summary>
    /// エンティティのうち、Actorの描画。
    /// アニメーションを制御したり座標を動かしたりする。
    /// </summary>
	public class ActorView : MonoBehaviour
	{
		[Inject]
		public void Construct(GridMovement gridMovement)
		{
			gridMovement.Position
				.Subscribe(position => Move(position))
				.AddTo(this);
		}

		private void Move(Vector2Int position)
        {
			transform.position = (Vector2)position;
        }
	}
}