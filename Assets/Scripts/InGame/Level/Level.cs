using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using yumehiko.LOF.Model;
using VContainer.Unity;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// レベル。攻略が要求される1つの単位。
    /// ダンジョン、プレイヤー、敵などのエンティティから成る。
    /// </summary>
	public class Level : IStartable
    {
        private readonly Dungeon dungeon;
        private readonly EntitySpawner entitySpawner;
        private readonly Turn turn;

        [Inject]
        public Level(Dungeon dungeon, EntitySpawner entitySpawner, Turn turn)
        {
            Debug.Log("level");
            this.dungeon = dungeon;
            this.entitySpawner = entitySpawner;
            this.turn = turn;
        }

        public void Start()
        {
            entitySpawner.SpawnEntities(dungeon.EntitySpawnPoints);
            turn.StartTurnLoop(entitySpawner.Player, entitySpawner.Enemies).Forget();
        }

        public void EndLevel()
        {
            turn.EndTurnLoop();
        }
    }
}