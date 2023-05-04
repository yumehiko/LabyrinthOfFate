using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LoF.GameLogic.Dungeon;
using LoF.GameLogic.Dungeon.Material;
using LoF.GameLogic.Entity.Actor;
using UniRx;

namespace LoF.GameLogic
{
    /// <summary>
    ///     レベル。攻略が要求される1つの単位。
    ///     ダンジョン、プレイヤー、敵などのエンティティから成る。
    /// </summary>
    public class Level : IDisposable
    {
        private readonly CompositeDisposable disposables = new();
        private readonly CancellationTokenSource levelCTS = new();

        private readonly Subject<LevelEndStat> onEnd = new();
        private readonly DungeonView view;

        public Level(
            DungeonAsset asset,
            DungeonView view,
            Actors actors,
            IReadOnlyList<ActorProfile> enemyProfiles,
            CancellationToken adventureCancelToken)
        {
            Terrain = asset.Terrain;
            Actors = actors;
            EnemyProfiles = enemyProfiles;
            this.view = view;
            Turn = new Turn();

            //ダンジョンの見かけや経路を生成
            Terrain.MakePathFinder();
            view.SetTiles(Terrain);
            actors.SpawnActors(asset.ActorSpawnPoints, this);

            //レベルの終了処理を登録
            _ = Turn.OnDefeatAllEnemies
                .First()
                .Subscribe(_ => EndLevel(LevelEndStat.Beat, adventureCancelToken).Forget())
                .AddTo(disposables);

            _ = Turn.OnPlayerIsDead
                .First()
                .Subscribe(_ => EndLevel(LevelEndStat.Lose, adventureCancelToken).Forget())
                .AddTo(disposables);
        }

        public Dungeon.Terrain Terrain { get; }
        public Turn Turn { get; }
        public Actors Actors { get; }
        public IReadOnlyList<ActorProfile> EnemyProfiles { get; }
        public IObservable<LevelEndStat> OnEnd => onEnd;

        public void Dispose()
        {
            Turn.Dispose();
            levelCTS?.Dispose();
            disposables?.Dispose();
        }

        public async UniTask StartLevel(CancellationToken adventureCancelToken)
        {
            await view.Show().ToUniTask(TweenCancelBehaviour.Complete, adventureCancelToken);
            Turn.StartTurnLoop(Actors, levelCTS.Token).Forget();
        }

        /// <summary>
        ///     このレベルを終了する。
        ///     レベルを片付けて、破棄処理をする。
        /// </summary>
        private async UniTask EndLevel(LevelEndStat endStat, CancellationToken adventureCancelToken)
        {
            Turn.Dispose();
            levelCTS.Cancel();
            levelCTS.Dispose();
            disposables.Dispose();
            await view.Hide().ToUniTask(cancellationToken: adventureCancelToken);
            Actors.ClearActorsWithoutPlayer();
            onEnd.OnNext(endStat);
        }
    }

    public enum LevelEndStat
    {
        None,
        Lose,
        Beat,
        Runaway
    }
}