using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.View
{
	/// <summary>
    /// タイルをセットする。
    /// </summary>
	public class TileSetter : MonoBehaviour
	{
		[SerializeField] private Tilemap tilemap;
		[SerializeField] private List<TileBase> tiles;

		public void SetTile(FloorTile tile)
        {
			var id = Random.Range(0, tiles.Count);
			tilemap.SetTile((Vector3Int)tile.Position, tiles[id]);
		}
	}
}