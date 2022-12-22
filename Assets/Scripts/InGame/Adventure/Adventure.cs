using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// 一連の冒険。
    /// </summary>
    public class Adventure : IDisposable, IInitializable
    {
        public Level CurrentLevel { get; private set; }

        private readonly DungeonView dungeonView;
        private readonly ActorPresenters actorPresenters;
        private readonly InfoUI infoUI;
        private readonly AdventureProgress progress;
        private IActorBrain player => actorPresenters.Player;

        private readonly CancellationTokenSource adventureCanncelation = new CancellationTokenSource();
        private readonly CompositeDisposable adventureDisposable = new CompositeDisposable();
        private IDisposable levelDisposable;

        [Inject]
        public Adventure(
            ActorPresenters actorPresenters,
            DungeonView dungeonView,
            InfoUI infoUI,
            AdventureProgress progress)
        {
            this.actorPresenters = actorPresenters;
            this.dungeonView = dungeonView;
            this.infoUI = infoUI;
            this.progress = progress;
        }

        public void Initialize()
        {
            actorPresenters.SpawnPlayer(progress.PlayerProfile, this);
            _ = actorPresenters.Models.Player.IsDied
                .Where(isTrue => isTrue)
                .Subscribe(_ => CurrentLevel.LoseLevel(adventureCanncelation.Token).Forget())
                .AddTo(adventureDisposable);
            infoUI.Initialize(actorPresenters.Models.Player);

            StartNewLevel();
        }

        public void Dispose()
        {
            adventureDisposable.Dispose();
            levelDisposable?.Dispose();
            CurrentLevel?.Dispose();
        }

        /// <summary>
        /// 新たなレベルを開始。
        /// </summary>
        public void StartNewLevel()
        {
            levelDisposable?.Dispose();
            CurrentLevel?.Dispose();
            var levelAsset = progress.PickNextLevelAssets();
            CurrentLevel = new Level(levelAsset.DungeonAsset, dungeonView, actorPresenters, levelAsset.EnemyProfiles);
            infoUI.SetLevel(CurrentLevel);
            levelDisposable = CurrentLevel.OnBeatLevel
                .First().Subscribe(_ => BeatLevel().Forget());
            CurrentLevel.StartLevel(adventureCanncelation.Token).Forget();
        }

        private async UniTaskVoid BeatLevel()
        {
            await CurrentLevel.BeatLevel(adventureCanncelation.Token);
            player.Heal(1000);
            //TODO:ここでリワードを与えたりする
            //TODO:レベルを打ち倒さずに逃げたRunDownLevel()関数も欲しい。
            StartNewLevel();
        }
    }
}