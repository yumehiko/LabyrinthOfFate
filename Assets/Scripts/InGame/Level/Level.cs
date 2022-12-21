using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;
using yumehiko.LOF.Model;
using VContainer.Unity;
using UniRx;
using System;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// レベル。攻略が要求される1つの単位。
    /// ダンジョン、プレイヤー、敵などのエンティティから成る。
    /// </summary>
	public class Level : IDisposable
    {
        public Dungeon Dungeon { get; }
        public Actors Actors { get; }

        private readonly Turn turn;
        private readonly CancellationTokenSource levelCancellationTokenSource = new CancellationTokenSource();
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        [Inject]
        public Level(DungeonAsset dungeonAsset, Actors actors, Turn turn, ActorBrains actorBrains, Camera mainCamera)
        {
            Dungeon = dungeonAsset.Dungeon;
            Actors = actors;
            this.turn = turn;

            var cameraPosition = new Vector3(-Dungeon.Bounds.position.x, -Dungeon.Bounds.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = cameraPosition;

            turn.StartTurnLoop(actorBrains.Player, actorBrains.Enemies, levelCancellationTokenSource.Token).Forget();
            actors.Player.IsDied
                .Where(isTrue => isTrue)
                .Subscribe(_ => EndLevel())
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        public void EndLevel()
        {
            levelCancellationTokenSource.Cancel();
            levelCancellationTokenSource.Dispose();
        }
    }
}