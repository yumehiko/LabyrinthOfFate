using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LoF.GameLogic.Dungeon.Material
{
    /// <summary>
    ///     ダンジョンを表す一式のデータ。
    ///     ダンジョンの地形と、エンティティスポーンポイントからなる。
    ///     Json形式で保存・解凍される。
    /// </summary>
    [Serializable]
    public class DungeonAsset
    {
        [FormerlySerializedAs("dungeon")] [SerializeField] private Terrain terrain;
        [SerializeField] private ActorSpawnPoints actorSpawnPoints;

        public DungeonAsset(Terrain terrain, ActorSpawnPoints actorSpawnPoints)
        {
            this.terrain = terrain;
            this.actorSpawnPoints = actorSpawnPoints;
        }

        public Terrain Terrain => terrain;
        public ActorSpawnPoints ActorSpawnPoints => actorSpawnPoints;
    }
}