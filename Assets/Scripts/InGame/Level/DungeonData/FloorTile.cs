using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// ダンジョンフロアのタイル1枚を表す。
    /// </summary>
    [Serializable]
    public struct FloorTile
    {
        public Vector2Int Position => position;
        public FloorType Type => type;

        [SerializeField] private Vector2Int position;
        [SerializeField] private FloorType type;

        public FloorTile(Vector2Int position, FloorType type)
        {
            this.position = position;
            this.type = type;
        }
    }
}