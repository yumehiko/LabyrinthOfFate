using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using VContainer;

namespace yumehiko.LOF
{
    public class GridMovement
    {
        public IReadOnlyReactiveProperty<Vector2Int> Position => position;

        private ReactiveProperty<Vector2Int> position;

        public GridMovement()
        {
            //TODO: ここでActorの初期位置とかをInjectして、反映する。
            Vector2Int initPosition = Vector2Int.zero;
            position = new ReactiveProperty<Vector2Int>(initPosition);
        }

        /// <summary>
        /// 現在の位置から、指定した方向に1マス進む。
        /// </summary>
        /// <param name="value"></param>
        public void StepTo(ActorDirection direction)
        {
            Vector2Int vector = DirectionToVector(direction);
            position.Value += vector;
        }

        private Vector2Int DirectionToVector(ActorDirection direction)
        {
            switch (direction)
            {
                case ActorDirection.None: return Vector2Int.zero;
                case ActorDirection.Up: return Vector2Int.up;
                case ActorDirection.Down: return Vector2Int.down;
                case ActorDirection.Right: return Vector2Int.right;
                case ActorDirection.Left: return Vector2Int.left;
                case ActorDirection.UpRight: return new Vector2Int(1, 1);
                case ActorDirection.UpLeft: return new Vector2Int(-1, 1);
                case ActorDirection.DownRight: return new Vector2Int(1, -1);
                case ActorDirection.DownLeft: return new Vector2Int(-1, -1);
                default: throw new InvalidOperationException();
            }
        }
    }
}