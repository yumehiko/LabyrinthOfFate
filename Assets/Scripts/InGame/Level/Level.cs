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
	public class Level
    {
        public Dungeon Dungeon { get; }
        public Actors Actors { get; }
        private readonly Turn turn;

        [Inject]
        public Level(DungeonAsset dungeonAsset, Actors actors, Turn turn, ActorBrains actorBrains, Camera mainCamera)
        {
            Dungeon = dungeonAsset.Dungeon;
            Actors = actors;
            this.turn = turn;

            var cameraPosition = new Vector3(-Dungeon.Bounds.position.x, -Dungeon.Bounds.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = cameraPosition;

            turn.StartTurnLoop(actorBrains.Player, actorBrains.Enemies).Forget();
        }

        public void EndLevel()
        {
            turn.EndTurnLoop();
        }
    }
}