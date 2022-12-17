﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar.Options;
using AStar;
using AStar.Collections;
using AStar.Heuristics;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// このフロアの地形から、経路情報を作成・管理する。
    /// Astarアルゴリズムへの橋渡しをしている。
    /// </summary>
    public class DungeonPathFinder
    {
        private short[,] tiles;
        private WorldGrid worldGrid;
        private PathFinder pathfinder;

        public DungeonPathFinder(Dungeon dungeon)
        {
            var pathfinderOptions = new PathFinderOptions();
            tiles = MakeTiles(dungeon);
            worldGrid = new WorldGrid(tiles);
            pathfinder = new PathFinder(worldGrid, pathfinderOptions);
        }

        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end) => pathfinder.FindPath(start, end);

        private short[,] MakeTiles(Dungeon dungeon)
        {
            var size = dungeon.Bounds.size;
            var tiles = new short[size.x, size.y];
            foreach (var dungeonTile in dungeon)
            {
                short tile = GetTileNumber(dungeonTile.Type);
                tiles[dungeonTile.Position.x, dungeonTile.Position.y] = tile;
            }

            return tiles;
        }

        private short GetTileNumber(TileType type)
        {
            switch (type)
            {
                case TileType.None: return 0;
                case TileType.Empty: return 1;
                case TileType.Wall: return 0;
                case TileType.BorderWall: return 0;
                default: throw new System.Exception("未定義のタイルタイプ。");
            }
        }
    }
}