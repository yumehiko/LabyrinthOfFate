using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace yumehiko.LOF.Model
{
    [Serializable]
    public class Floor : IReadOnlyList<FloorTile>
	{
        [SerializeField] private List<FloorTile> tiles;

        public Floor(List<FloorTile> tiles)
        {
            this.tiles = tiles;
        }

        public FloorTile this[int index] => tiles[index];

        public int Count => tiles.Count;

        public IEnumerator<FloorTile> GetEnumerator() => tiles.GetEnumerator();

        /// <summary>
        /// 指定した地点の地形を返す。
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public FloorType GetTerrainType(Vector2Int position)
        {
            //MEMO:positionにtileがない場合は多分例外が飛ぶ
            var findResult = tiles.Find(tile => tile.Position == position);
            return findResult.Type;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}