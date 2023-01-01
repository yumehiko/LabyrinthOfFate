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
using yumehiko.Resident;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// 一連の冒険。レベルの生成・終了・プレイヤーの敗北処理など。
    /// </summary>
    public class Adventure : IDisposable, IInitializable
    {
        public Level CurrentLevel { get; private set; }
        public Player Player { get; }

        private readonly DungeonView dungeonView;
        private readonly Actors actors;
        private readonly InfoUI infoUI;
        private readonly AdventureProgress progress;
        private readonly Rewards rewards;

        private readonly CancellationTokenSource adventureCanncelation = new CancellationTokenSource();
        private readonly CompositeDisposable adventureDisposable = new CompositeDisposable();
        private IDisposable levelDisposable;

        [Inject]
        public Adventure(
            Player player,
            Actors actors,
            DungeonView dungeonView,
            Rewards rewards,
            InfoUI infoUI,
            AdventureProgress progress)
        {
            Player = player;
            this.actors = actors;
            this.dungeonView = dungeonView;
            this.rewards = rewards;
            this.infoUI = infoUI;
            this.progress = progress;
        }

        public void Initialize()
        {
            Player.Initialize(this);
            infoUI.Initialize(Player);

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
            CurrentLevel = new Level(levelAsset.DungeonAsset, dungeonView, actors, levelAsset.EnemyProfiles, adventureCanncelation.Token);
            infoUI.SetLevel(CurrentLevel);
            levelDisposable = CurrentLevel.OnEnd
                .First()
                .Subscribe(endStat => EndLevel(endStat).Forget());
            CurrentLevel.StartLevel(adventureCanncelation.Token).Forget();
        }

        private async UniTaskVoid EndLevel(LevelEndStat endStat)
        {
            if(endStat == LevelEndStat.Lose)
            {
                Debug.Log($"プレイヤー敗北時の処理を入れる。とりあえずシーンリセット");
                await LoadManager.RequireResetScene();
                return;
            }

            Player.Heal(1000);
            Player.Model.Status.Buffs.CountLevelLifetime();
            Player.Model.Status.RefleshEnergy();

            try
            {
                if (endStat == LevelEndStat.Beat)
                {
                    await rewards.WaitUntilePickReward(actors.Player.InventoryUI, adventureCanncelation.Token);
                }
            }
            finally
            {
                StartNewLevel();
            }
        }
    }
}