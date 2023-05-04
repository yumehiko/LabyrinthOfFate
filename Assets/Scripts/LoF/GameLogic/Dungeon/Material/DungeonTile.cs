using System;
using UnityEngine;

namespace LoF.GameLogic.Dungeon.Material
{
    /// <summary>
    ///     ダンジョンフロアのタイル1枚を表す。
    /// </summary>
    [Serializable]
    public struct DungeonTile
    {
        [SerializeField] private Vector2Int position;
        [SerializeField] private TileType type;

        public DungeonTile(Vector2Int position, TileType type)
        {
            this.position = position;
            this.type = type;
        }

        public Vector2Int Position => position;
        public TileType Type => type;
    }
}