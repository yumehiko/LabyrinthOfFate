using System;
using System.Linq;
using AStar;
using AStar.Options;
using LoF.GameLogic.Dungeon.Material;
using UnityEngine;

namespace LoF.GameLogic.Dungeon
{
    /// <summary>
    ///     このフロアの地形から、経路情報を作成・管理する。
    ///     AStarアルゴリズムへの橋渡しをしている。
    /// </summary>
    public class DungeonPathFinder
    {
        private readonly PathFinder pathfinder;

        public DungeonPathFinder(Terrain terrain)
        {
            var pathfinderOptions = new PathFinderOptions();
            var tiles = MakePathTiles(terrain);
            var worldGrid = new WorldGrid(tiles);
            pathfinder = new PathFinder(worldGrid, pathfinderOptions);
        }

        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end)
        {
            return pathfinder.FindPath(new Position(start.x, start.y), new Position(end.x, end.y))
                .Select(position => new Vector2Int(position.Row, position.Column))
                .ToArray();
        }

        private short[,] MakePathTiles(Terrain terrain)
        {
            var size = terrain.Bounds.size;
            var tiles = new short[size.x, size.y];
            foreach (var dungeonTile in terrain)
            {
                var tile = GetTileNumber(dungeonTile.Type);
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
                default: throw new Exception("未定義のタイルタイプ。");
            }
        }
    }
}