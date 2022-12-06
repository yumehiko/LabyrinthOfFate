using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace yumehiko.LOF
{
    /// <summary>
    /// Actorの描写を管理する。
    /// </summary>
	public class ActorVisual : MonoBehaviour
	{
        /// <summary>
        /// 指定したポイントへステップするアニメーションを再生する。
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public Tweener StepAnimation(Vector2 endPoint, float duration)
        {
            return transform.DOMove(endPoint, duration).SetLink(gameObject);
        }
    }
}