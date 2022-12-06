using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using DG.Tweening;

namespace yumehiko.LOF
{
    /// <summary>
    /// Grid上の衝突判定のためなどのColliderを動かす。
    /// </summary>
    public class GridMovement : MonoBehaviour
    {
        /// <summary>
        /// 現在の位置から、指定した方向に1マス進む。
        /// </summary>
        /// <param name="value"></param>
        public Vector2 StepTo(ActorDirection direction)
        {
            Vector2 endPoint = (Vector2)transform.position + DirectionToVector(direction);
            transform.position = endPoint;
            return endPoint;
        }

        /// <summary>
        /// 指定した方向に存在するエンティティを返す。存在しない場合はnullを返す。
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public IEntity CheckEntityTo(ActorDirection direction)
        {
            Vector2 point = (Vector2)transform.position + DirectionToVector(direction);
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            if(hit.collider == null)
            {
                return null;
            }
            return hit.collider.GetComponent<IEntity>();
        }

        private Vector2 DirectionToVector(ActorDirection direction)
        {
            switch (direction)
            {
                case ActorDirection.None: return Vector2.zero;
                case ActorDirection.Up: return Vector2.up;
                case ActorDirection.Down: return Vector2.down;
                case ActorDirection.Right: return Vector2.right;
                case ActorDirection.Left: return Vector2.left;
                case ActorDirection.UpRight: return new Vector2(1, 1);
                case ActorDirection.UpLeft: return new Vector2(-1, 1);
                case ActorDirection.DownRight: return new Vector2(1, -1);
                case ActorDirection.DownLeft: return new Vector2(-1, -1);
                default: throw new InvalidOperationException();
            }
        }
    }
}