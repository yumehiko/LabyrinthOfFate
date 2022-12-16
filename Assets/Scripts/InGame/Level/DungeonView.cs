﻿using System.Collections;
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
        [SerializeField] private TileSetter wallTileSetter;
        [SerializeField] private TileSetter borderWallTileSetter;

        [Inject]
        public void Construct(Dungeon dungeon)
        {
            SetTiles(dungeon.Floor);
        }

        private void SetTiles(Floor floor)
        {
            foreach (var tile in floor)
            {
                SetTile(tile);
            }
        }

        private void SetTile(FloorTile tile)
        {
            switch (tile.Type)
            {
                case FloorType.Empty: return;
                case FloorType.Wall:
                    wallTileSetter.SetTile(tile);
                    return;
                case FloorType.BorderWall:
                    borderWallTileSetter.SetTile(tile);
                    return;
                default: throw new System.Exception("未定義の地形タイプ");
            }
        }
    }
}