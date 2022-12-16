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
        [SerializeField] private Tilemap terrainMap;
        [SerializeField] private Tilemap spawnPointMap;

        public void WriteJson()
        {
            var dungeon = GetDungeonInstance();
            string rawJson = JsonUtility.ToJson(dungeon, true);
            using (StreamWriter writer = new StreamWriter(Application.dataPath + $"/Json/Dungeon/{jsonName}.json", false))
            {
                writer.Write(rawJson);
            }
        }

        private Dungeon GetDungeonInstance()
        {
            terrainMap.CompressBounds();
            BoundsInt bounds = terrainMap.cellBounds;
            var floor = GetFloor(bounds);
            var spawnPoints = GetEntitySpawnPoints(bounds);
            var dungeon = new Dungeon(floor, spawnPoints, bounds);
            return dungeon;
        }

        private Floor GetFloor(BoundsInt bounds)
        {
            //TODO: 外周にひとまわりBorderWallを敷けば安全
            //bounds.size += new Vector3Int(2, 2, 0);
            TileBase[] allTiles = terrainMap.GetTilesBlock(bounds);
            List<FloorTile> dungeonTiles = new List<FloorTile>();

            for (int y = 0; y < bounds.size.y; y++)
            {
                for (int x = 0; x < bounds.size.x; x++)
                {
                    TileBase tileBase = allTiles[x + y * bounds.size.x]; //MEMO: allTilesは1次元配列なので、indexはこのように取得している。
                    FloorType type = GetTerrainTypeFromTile(tileBase);
                    var dungeonTile = new FloorTile(new Vector2Int(x, y), type);
                    dungeonTiles.Add(dungeonTile);
                }
            }

            var floor = new Floor(dungeonTiles);
            return floor;
        }

        private EntitySpawnPoints GetEntitySpawnPoints(BoundsInt bounds)
        {
            TileBase[] allTiles = spawnPointMap.GetTilesBlock(bounds);
            var spawnPointList = new List<EntitySpawnPoint>();

            for (short x = 0; x < bounds.size.x; x++)
            {
                for (short y = 0; y < bounds.size.y; y++)
                {
                    TileBase tileBase = allTiles[x + y * bounds.size.x];
                    if (tileBase == null)
                    {
                        continue;
                    }
                    var type = GetEntitySpawnPointType(tileBase);
                    var point = new EntitySpawnPoint(x, y, type);
                    spawnPointList.Add(point);
                }
            }

            var spawnPoints = new EntitySpawnPoints(spawnPointList);
            return spawnPoints;
        }

        private FloorType GetTerrainTypeFromTile(TileBase tile)
        {
            if (tile == null)
            {
                return FloorType.BorderWall;
            }

            switch (tile.name)
            {
                case "Floor": return FloorType.Empty;
                case "Wall": return FloorType.Wall;
                case "BorderWall": return FloorType.BorderWall;
                default: throw new System.Exception("未定義");
            }
        }

        private ActorType GetEntitySpawnPointType(TileBase tile)
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