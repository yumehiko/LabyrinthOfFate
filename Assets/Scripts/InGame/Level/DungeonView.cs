using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;
using VContainer;

namespace yumehiko.LOF.View
{
    /// <summary>
    /// ゲーム中のダンジョンのView。
    /// </summary>
	public class DungeonView : MonoBehaviour
    {
        [SerializeField] private TileViewSetter wallTileSetter;
        [SerializeField] private TileViewSetter borderWallTileSetter;

        [Inject]
        public void Construct(Dungeon dungeon)
        {
            SetTiles(dungeon);
        }

        private void SetTiles(Dungeon floor)
        {
            foreach (var tile in floor)
            {
                SetTile(tile);
            }
        }

        private void SetTile(DungeonTile tile)
        {
            switch (tile.Type)
            {
                case TileType.Empty: return;
                case TileType.Wall:
                    wallTileSetter.SetTile(tile);
                    return;
                case TileType.BorderWall:
                    borderWallTileSetter.SetTile(tile);
                    return;
                default: throw new System.Exception("未定義の地形タイプ");
            }
        }
    }
}