using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// ダンジョンを表す一式のデータ。
    /// ダンジョンの地形と、エンティティスポーンポイントからなる。
    /// Json形式で保存・解凍される。
    /// </summary>
    [Serializable]
    public class DungeonAsset
    {
        [SerializeField] private Dungeon dungeon;
        [SerializeField] private ActorSpawnPoints actorSpawnPoints;

        public Dungeon Dungeon => dungeon;
        public ActorSpawnPoints ActorSpawnPoints => actorSpawnPoints;

        public DungeonAsset(Dungeon dungeon, ActorSpawnPoints actorSpawnPoints)
        {
            this.dungeon = dungeon;
            this.actorSpawnPoints = actorSpawnPoints;
        }
    }
}