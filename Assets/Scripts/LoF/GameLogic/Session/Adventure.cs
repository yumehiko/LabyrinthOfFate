using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LoF.GameLogic.Dungeon;
using LoF.GameLogic.Entity.Actor;
using LoF.GameLogic.Entity.Actor.Brains;
using LoF.UI.Info;
using LoF.UI.Reward;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using yumehiko.Resident;

namespace LoF.GameLogic.Session
{
    /// <summary>
    ///     一連の冒険。レベルの生成・終了・プレイヤーの敗北処理など。
    /// </summary>
    public class Adventure : IDisposable, IInitializable
    {
        private readonly Actors actors;

        private readonly CancellationTokenSource adventureCanncelation = new();
        private readonly CompositeDisposable adventureDisposable = new();

        private readonly DungeonView dungeonView;
        private readonly InfoUI infoUI;
        private readonly AdventureProgress progress;
        private readonly RewardsUI rewardsUI;
        private IDisposable levelDisposable;

        [Inject]
        public Adventure(
            Player player,
            Actors actors,
            DungeonView dungeonView,
            RewardsUI rewardsUI,
            InfoUI infoUI,
            AdventureProgress progress)
        {
            Player = player;
            this.actors = actors;
            this.dungeonView = dungeonView;
            this.rewardsUI = rewardsUI;
            this.infoUI = infoUI;
            this.progress = progress;
        }

        public Level CurrentLevel { get; private set; }
        public Player Player { get; }

        public void Dispose()
        {
            adventureDisposable.Dispose();
            levelDisposable?.Dispose();
            CurrentLevel?.Dispose();
        }

        public void Initialize()
        {
            Player.Initialize(this);
            infoUI.Initialize(Player);

            StartNewLevel();
        }

        /// <summary>
        ///     新たなレベルを開始。
        /// </summary>
        public void StartNewLevel()
        {
            levelDisposable?.Dispose();
            CurrentLevel?.Dispose();
            var levelAsset = progress.PickNextLevelAssets();
            CurrentLevel = new Level(levelAsset.DungeonAsset, dungeonView, actors, levelAsset.EnemyProfiles,
                adventureCanncelation.Token);
            infoUI.SetLevel(CurrentLevel);
            levelDisposable = CurrentLevel.OnEnd
                .First()
                .Subscribe(endStat => EndLevel(endStat).Forget());
            CurrentLevel.StartLevel(adventureCanncelation.Token).Forget();
        }

        private async UniTaskVoid EndLevel(LevelEndStat endStat)
        {
            if (endStat == LevelEndStat.Lose)
            {
                Debug.Log("プレイヤー敗北時の処理を入れる。とりあえずシーンリセット");
                await LoadManager.RequireResetScene();
                return;
            }

            Player.Heal(1000);
            Player.Model.Status.Buffs.CountLevelLifetime();
            Player.Model.Status.RefleshEnergy();

            try
            {
                if (endStat == LevelEndStat.Beat)
                    await rewardsUI.WaitUntilePickReward(actors.Player.InventoryUI, adventureCanncelation.Token);
            }
            finally
            {
                StartNewLevel();
            }
        }
    }
}