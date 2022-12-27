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
        public Player Player { get; }

        private readonly DungeonView dungeonView;
        private readonly ActorPresenters actorPresenters;
        private readonly InfoUI infoUI;
        private readonly AdventureProgress progress;
        private readonly Rewards rewards;

        private readonly CancellationTokenSource adventureCanncelation = new CancellationTokenSource();
        private readonly CompositeDisposable adventureDisposable = new CompositeDisposable();
        private IDisposable levelDisposable;

        [Inject]
        public Adventure(
            Player player,
            ActorPresenters actorPresenters,
            DungeonView dungeonView,
            Rewards rewards,
            InfoUI infoUI,
            AdventureProgress progress)
        {
            Player = player;
            this.actorPresenters = actorPresenters;
            this.dungeonView = dungeonView;
            this.rewards = rewards;
            this.infoUI = infoUI;
            this.progress = progress;
        }

        public void Initialize()
        {
            Player.Initialize(this);
            infoUI.Initialize(Player.Model);

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
            CurrentLevel = new Level(levelAsset.DungeonAsset, dungeonView, actorPresenters, levelAsset.EnemyProfiles, adventureCanncelation.Token);
            infoUI.SetLevel(CurrentLevel);
            levelDisposable = CurrentLevel.OnEnd
                .First()
                .Subscribe(endStat => EndLevel(endStat).Forget());
            CurrentLevel.StartLevel(adventureCanncelation.Token).Forget();
        }

        private async UniTaskVoid EndLevel(LevelEndStat endStat)
        {
            actorPresenters.Player.Heal(1000);
            //TODO:ここでリワードを与えたりする
            if (endStat == LevelEndStat.Beat)
            {
                await rewards.WaitUntilePickReward(actorPresenters.Player.Inventory, adventureCanncelation.Token);
            }
            StartNewLevel();
        }
    }
}