using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using yumehiko.LOF.Model;
using System.IO;

namespace yumehiko.LOF.Editor
{
    public class DungeonEditor : MonoBehaviour
    {
        [ContextMenuItem("WriteJson", "WriteJson")]
        [SerializeField] private string jsonName;
        [SerializeField] private Tilemap dungeonMap;
        [SerializeField] private Tilemap spawnPointMap;

        public void WriteJson()
        {
            var dungeonAsset = GenerateDungeonAsset();
            string rawJson = JsonUtility.ToJson(dungeonAsset, true);
            using (StreamWriter writer = new StreamWriter(Application.dataPath + $"/Json/Dungeon/{jsonName}.json", false))
            {
                writer.Write(rawJson);
            }
        }

        private DungeonAsset GenerateDungeonAsset()
        {
            dungeonMap.CompressBounds();
            BoundsInt bounds = dungeonMap.cellBounds;
            var dungeon = GetDungeon(bounds);
            var spawnPoints = GetActorSpawnPoints(bounds);
            var dungeonAsset = new DungeonAsset(dungeon, spawnPoints);
            return dungeonAsset;
        }

        private Dungeon GetDungeon(BoundsInt bounds)
        {
            //TODO: 外周にひとまわりBorderWallを敷けば安全
            //bounds.size += new Vector3Int(2, 2, 0);
            TileBase[] allTiles = dungeonMap.GetTilesBlock(bounds);
            List<DungeonTile> dungeonTiles = new List<DungeonTile>();

            for (int y = 0; y < bounds.size.y; y++)
            {
                for (int x = 0; x < bounds.size.x; x++)
                {
                    TileBase tileBase = allTiles[x + y * bounds.size.x]; //MEMO: allTilesは1次元配列なので、indexはこのように取得している。
                    TileType type = GetTileType(tileBase);
                    var dungeonTile = new DungeonTile(new Vector2Int(x, y), type);
                    dungeonTiles.Add(dungeonTile);
                }
            }

            var dungeon = new Dungeon(dungeonTiles, bounds);
            return dungeon;
        }

        private ActorSpawnPoints GetActorSpawnPoints(BoundsInt bounds)
        {
            TileBase[] allTiles = spawnPointMap.GetTilesBlock(bounds);
            var spawnPointList = new List<ActorSpawnPoint>();

            for (short x = 0; x < bounds.size.x; x++)
            {
                for (short y = 0; y < bounds.size.y; y++)
                {
                    TileBase tileBase = allTiles[x + y * bounds.size.x];
                    if (tileBase == null)
                    {
                        continue;
                    }
                    var type = GetActorSpawnPointType(tileBase);
                    var point = new ActorSpawnPoint(x, y, type);
                    spawnPointList.Add(point);
                }
            }

            var spawnPoints = new ActorSpawnPoints(spawnPointList);
            return spawnPoints;
        }

        private TileType GetTileType(TileBase tile)
        {
            if (tile == null)
            {
                return TileType.BorderWall;
            }

            switch (tile.name)
            {
                case "Floor": return TileType.Empty;
                case "Wall": return TileType.Wall;
                case "BorderWall": return TileType.BorderWall;
                default: throw new System.Exception("未定義");
            }
        }

        private ActorType GetActorSpawnPointType(TileBase tile)
        {
            switch(tile.name)
            {
                case "PlayerSpawnPoint": return ActorType.Player;
                case "EnemySpawnPoint": return ActorType.Enemy;
                default: throw new System.Exception("未定義");
            }
        }
    }
}