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
        public void Construct(DungeonAsset dungeonAsset)
        {
            SetTiles(dungeonAsset.Dungeon);
        }

        private void SetTiles(Dungeon dungeon)
        {
            foreach (var tile in dungeon)
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