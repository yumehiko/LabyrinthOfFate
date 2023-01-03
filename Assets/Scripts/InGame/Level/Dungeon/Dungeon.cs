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

        /// <summary>
        /// 2点を表すグリッド直線を引き、その直線を表す座標のリストを返す。
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<Vector2Int> GetLineCoordinates(Vector2Int origin, Vector2Int target)
        {
            var coordinates = new List<Vector2Int>();
            int deltaX = Mathf.Abs(target.x - origin.x);
            int deltaY = Mathf.Abs(target.y - origin.y);
            int stepX = (origin.x < target.x) ? 1 : -1;
            int stepY = (origin.y < target.y) ? 1 : -1;
            int error = deltaX - deltaY;

            while(true)
            {
                coordinates.Add(origin);
                if(origin == target)
                {
                    break;
                }

                int e2 = error * 2;
                if(e2 > -deltaY)
                {
                    error -= deltaY;
                    origin.x += stepX;
                }
                if(e2 < deltaX)
                {
                    error += deltaX;
                    origin.y += stepY;
                }
            }

            return coordinates;
        }
    }
}