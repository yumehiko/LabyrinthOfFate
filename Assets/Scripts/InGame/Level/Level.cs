using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
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
        public Turn Turn { get; }
        public ActorModels Actors { get; }
        public IReadOnlyList<ActorProfile> EnemyProfiles { get; }
        public IObservable<Unit> OnBeatLevel => actorPresenters.OnDefeatAllEnemy;

        private readonly DungeonView view;
        private readonly ActorPresenters actorPresenters;
        private readonly CancellationTokenSource levelCancellationTokenSource = new CancellationTokenSource();

        public Level(DungeonAsset asset, DungeonView view, ActorPresenters actorPresenters, IReadOnlyList<ActorProfile> enemyProfiles)
        {
            Dungeon = asset.Dungeon;
            Actors = actorPresenters.Models;
            this.actorPresenters = actorPresenters;
            this.EnemyProfiles = enemyProfiles;
            this.view = view;
            this.Turn = new Turn();

            //ダンジョンの見かけや経路を生成
            Dungeon.MakePathFinder();
            view.SetTiles(Dungeon);
            actorPresenters.SpawnActors(asset.ActorSpawnPoints, this);
        }

        public void Dispose()
        {
            levelCancellationTokenSource?.Dispose();
        }

        public async UniTask StartLevel(CancellationToken adventureCancelToken)
        {
            await view.Show().ToUniTask(TweenCancelBehaviour.Complete, adventureCancelToken);
            Turn.StartTurnLoop(actorPresenters.Player, actorPresenters.Enemies, levelCancellationTokenSource.Token).Forget();
        }

        /// <summary>
        /// このレベルに敗北する。
        /// </summary>
        public async UniTask LoseLevel(CancellationToken adventureCancelToken)
        {
            levelCancellationTokenSource.Cancel();
            levelCancellationTokenSource.Dispose();
            await view.Hide().ToUniTask(cancellationToken: adventureCancelToken);
            actorPresenters.ClearActorsWithoutPlayer();
        }

        /// <summary>
        /// このレベルに勝利する。
        /// レベルを片付けて、破棄処理をする。
        /// </summary>
        public async UniTask BeatLevel(CancellationToken adventureCancelToken)
        {
            levelCancellationTokenSource.Cancel();
            levelCancellationTokenSource.Dispose();
            await view.Hide().ToUniTask(cancellationToken: adventureCancelToken);
            actorPresenters.ClearActorsWithoutPlayer();
        }
    }
}