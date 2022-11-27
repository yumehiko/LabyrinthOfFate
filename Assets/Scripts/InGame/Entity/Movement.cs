using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using yumehiko.LOF;


namespace yumehiko.LOF.Old
{
    /// <summary>
    /// Transformをステップ単位で動かす。
    /// TODO: Vector2Int式に変更する。MonoBehaviour多分いらない。
    /// </summary>
    public class Movement : MonoBehaviour
    {
        [SerializeField] private LayerMask moveMask;

        /// <summary>
        /// 現在の位置から、目標の座標へ向かって、1ステップ進む。
        /// </summary>
        public void StepToPosition(Vector3 targetPosition, float duration)
        {
            ActorDirection direction = PositionToDirection(transform.position, targetPosition);
            MoveToDirection(direction, duration);
        }

        /// <summary>
        /// 現在の位置から、指定方向へ向かって、1ステップ進む。
        /// </summary>
        /// <param name="point"></param>
        public void MoveToDirection(ActorDirection direction, float duration)
        {
            const float unit = 1.92f;
            Vector2 angle = DirectionToVector2(direction);

            if (!CheckCanMove(angle))
            {
                //TODO ここで壁にぶつかるアニメーションとかを入れておく
                Debug.Log("CantMove");
                return;
            }

            _ = transform.DOMove(angle * unit, duration)
                .SetEase(Ease.InOutQuart)
                .SetRelative();
        }

        /// <summary>
        /// 指定された座標に進めるかチェックする。
        /// </summary>
        /// <param name="moveOffset">現在地から目標地までの方向と距離。</param>
        /// <returns>進めるならTrue</returns>
        public bool CheckCanMove(Vector2 angle)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, (Vector2)transform.position + angle, moveMask);
            return hit.collider == null;
        }

        /// <summary>
        /// ActorDirectionをVector2方向成分へ変換。
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector2 DirectionToVector2(ActorDirection direction)
        {
            switch (direction)
            {
                case ActorDirection.Up:
                    return Vector2.up;
                case ActorDirection.Down:
                    return Vector2.down;
                case ActorDirection.Right:
                    return Vector2.right;
                case ActorDirection.Left:
                    return Vector2.left;
                default:
                    return Vector2.zero;
            }
        }

        /// <summary>
        /// A, Bの座標をもとに、AからBに向かう正規化して丸めたVector2を返す。
        /// 返り値は、Vector2.up/down/right/leftと同義。
        /// </summary>
        /// <param name="position"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public static ActorDirection PositionToDirection(Vector2 position, Vector2 targetPosition)
        {
            Vector2 heading = targetPosition - position;
            float distance = heading.magnitude;
            Vector2 direction = heading / distance;

            int x = Mathf.RoundToInt(direction.x);
            if (x == 1)
            {
                return ActorDirection.Right;
            }

            if (x == -1)
            {
                return ActorDirection.Left;
            }

            int y = Mathf.RoundToInt(direction.y);
            if (y == 1)
            {
                return ActorDirection.Up;
            }

            if (y == -1)
            {
                return ActorDirection.Down;
            }

            return ActorDirection.None;
        }

    }
}