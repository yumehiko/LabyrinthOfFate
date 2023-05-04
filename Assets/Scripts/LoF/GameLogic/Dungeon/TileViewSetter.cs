using System.Collections.Generic;
using LoF.GameLogic.Dungeon.Material;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LoF.GameLogic.Dungeon
{
	/// <summary>
	///     タイルをセットする。
	/// </summary>
	public class TileViewSetter : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private List<TileBase> tiles;

        public void SetTile(DungeonTile tile)
        {
            var id = Random.Range(0, tiles.Count);
            tilemap.SetTile((Vector3Int)tile.Position, tiles[id]);
        }
    }
}