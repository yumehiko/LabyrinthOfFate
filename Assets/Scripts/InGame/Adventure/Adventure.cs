using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

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
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly InfoUI infoUI;

        private readonly DungeonAsset asset;
        private readonly PlayerProfile playerProfile;
        private readonly List<ActorProfile> enemyProfiles;

        [Inject]
        public Adventure(
            DungeonAsset firstDungeonAsset,
            PlayerProfile playerProfile,
            List<ActorProfile> enemyProfiles,
            ActorPresenters actorPresenters,
            DungeonView dungeonView,
            InfoUI infoUI)
        {
            this.asset = firstDungeonAsset;
            this.playerProfile = playerProfile;
            this.enemyProfiles = enemyProfiles;

            this.actorPresenters = actorPresenters;
            this.dungeonView = dungeonView;
            this.infoUI = infoUI;
        }

        public void Initialize()
        {
            StartAdventure(asset, playerProfile, enemyProfiles);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        /// <summary>
        /// 一連の冒険を開始する。
        /// </summary>
        public void StartAdventure(DungeonAsset firstLevel, PlayerProfile playerProfile, List<ActorProfile> enemyProfiles)
        {
            actorPresenters.SpawnPlayer(playerProfile, this);
            actorPresenters.Models.Player.IsDied
                .Where(isTrue => isTrue)
                .Subscribe(_ => CurrentLevel.EndLevel())
                .AddTo(disposables);

            infoUI.Initialize(actorPresenters.Models.Player);
            StartNewLevel(firstLevel, enemyProfiles);
        }

        /// <summary>
        /// 新たなレベルを開始。
        /// </summary>
        public void StartNewLevel(DungeonAsset asset, List<ActorProfile> enemyProfiles)
        {
            CurrentLevel = new Level(asset, dungeonView, actorPresenters, enemyProfiles);
            infoUI.SetLevel(CurrentLevel);
            CurrentLevel.StartLevel();
        }
    }
}