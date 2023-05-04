using System.Collections.Generic;
using LoF.GameLogic.Dungeon.Material;
using LoF.GameLogic.Entity.Actor;

namespace LoF.GameLogic.Session
{
	/// <summary>
	///     あるレベルを表す情報のまとまり。
	/// </summary>
	public class LevelAsset
    {
        public LevelAsset(DungeonAsset dungeonAsset, IReadOnlyList<ActorProfile> enemyProfiles)
        {
            this.DungeonAsset = dungeonAsset;
            this.EnemyProfiles = enemyProfiles;
        }

        public DungeonAsset DungeonAsset { get; }

        public IReadOnlyList<ActorProfile> EnemyProfiles { get; }
    }
}