using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// ダンジョン。あるレベルの地形を示す。
    /// </summary>
    [Serializable]
    public class Dungeon : IReadOnlyList<DungeonTile>
	{
        [SerializeField] private List<DungeonTile> tiles;
        [SerializeField] private BoundsInt bounds;

        public BoundsInt Bounds => bounds;

        private DungeonPathFinder pathFinder;

        public Dungeon(List<DungeonTile> tiles, BoundsInt bounds)
        {
            this.tiles = tiles;
            this.bounds = bounds;
        }

        public DungeonTile this[int index] => tiles[index];

        public int Count => tiles.Count;

        public IEnumerator<DungeonTile> GetEnumerator() => tiles.GetEnumerator();

        /// <summary>
        /// 指定した地点の地形を返す。
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public TileType GetTileType(Vector2Int position)
        {
            //MEMO:positionにtileがない場合は多分例外が飛ぶ
            var findResult = tiles.Find(tile => tile.Position == position);
            return findResult.Type;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void MakePathFinder()
        {
            pathFinder = new DungeonPathFinder(this);
        }

        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end) => pathFinder.FindPath(start, end);
    }
}