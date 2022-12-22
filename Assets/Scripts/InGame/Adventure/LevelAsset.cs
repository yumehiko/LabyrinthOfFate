using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Presenter;

namespace yumehiko.LOF.Model
{
	/// <summary>
    /// あるレベルを表す情報のまとまり。
    /// </summary>
	public class LevelAsset
	{
        public DungeonAsset DungeonAsset => dungeonAsset;
        public IReadOnlyList<ActorProfile> EnemyProfiles => enemyProfiles;

        private readonly DungeonAsset dungeonAsset;
        private readonly IReadOnlyList<ActorProfile> enemyProfiles;

        public LevelAsset(DungeonAsset dungeonAsset, IReadOnlyList<ActorProfile> enemyProfiles)
        {
            this.dungeonAsset = dungeonAsset;
            this.enemyProfiles = enemyProfiles;
        }
	}
}