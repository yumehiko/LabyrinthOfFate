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
        private readonly ActorSpawnPoints actorSpawnPoints;
        private readonly ActorBrains actorBrains;
        private readonly Turn turn;

        [Inject]
        public Level(Dungeon dungeon, ActorSpawnPoints actorSpawnPoints,  ActorBrains actorBrains, Turn turn, Camera mainCamera)
        {
            this.actorSpawnPoints = actorSpawnPoints;
            this.actorBrains = actorBrains;
            this.turn = turn;
            var cameraPosition = new Vector3(-dungeon.Bounds.position.x, -dungeon.Bounds.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = cameraPosition;
        }

        public void Start()
        {
            actorBrains.SpawnActors(actorSpawnPoints);
            turn.StartTurnLoop(actorBrains.Player, actorBrains.Enemies).Forget();
        }

        public void EndLevel()
        {
            turn.EndTurnLoop();
        }
    }
}