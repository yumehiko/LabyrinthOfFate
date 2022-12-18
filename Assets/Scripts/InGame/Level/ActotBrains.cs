using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using UniRx;
using System;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// 実行中のゲームの、全てのActorBrainのコレクション。
    /// </summary>
    public class ActorBrains : IDisposable
    {
        public IActorBrain Player => player;
        public IReadOnlyList<IActorBrain> Enemies => enemies;

        private IActorBrain player;
        private readonly List<IActorBrain> enemies = new List<IActorBrain>();
        private readonly ActorViews views;
        private readonly Actors models;

        private readonly List<ActorProfile> enemyProfiles;
        private readonly Dungeon dungeon;
        private readonly IActorProfile playerProfile;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        [Inject]
        public ActorBrains(
            Dungeon dungeon,
            PlayerProfile playerProfile,
            List<ActorProfile> enemyProfiles,
            Actors models,
            ActorViews views)
        {
            this.models = models;
            this.views = views;
            this.dungeon = dungeon;
            this.playerProfile = playerProfile;
            this.enemyProfiles = enemyProfiles;
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        public void SpawnActors(ActorSpawnPoints spawnPoints)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                SpawnActor(spawnPoint);
            }
            if (player == null)
            {
                throw new Exception("Playerが生成できなかった。");
            }
        }

        private void SpawnActor(ActorSpawnPoint spawnPoint)
        {
            switch (spawnPoint.Type)
            {
                case ActorType.Player:
                    if (player != null) return;
                    player = SpawnPlayer(spawnPoint);
                    return;
                case ActorType.Enemy:
                    SpawnEnemy(spawnPoint);
                    return;
            }
        }

        private IActorBrain SpawnPlayer(ActorSpawnPoint spawnPoint)
        {
            var body = models.SpawnPlayer(playerProfile, spawnPoint.Position);
            var view = views.SpawnActorView(spawnPoint, playerProfile.View);
            var brain = new Player(dungeon, models, body, view);
            disposables.Add(brain);
            return brain;

        }

        private IActorBrain SpawnEnemy(ActorSpawnPoint spawnPoint)
        {
            var id = UnityEngine.Random.Range(0, enemyProfiles.Count);
            var profile = enemyProfiles[id];
            var body = models.SpawnEnemy(profile, spawnPoint.Position);
            var view = views.SpawnActorView(spawnPoint, profile.View);
            var brain = SpawnBrain(profile.BrainType, body, view);
            enemies.Add(brain);
            body.IsDied
                .Where(isTrue => isTrue)
                .First()
                .Subscribe(_ => RemoveEnemy(brain, body, view))
                .AddTo(disposables);
            return brain;
        }

        private IActorBrain SpawnBrain(BrainType type, Actor body, IActorView view)
        {
            switch (type)
            {
                case BrainType.RandomStep:
                    return new RandomStepper(dungeon, models, body, view);
                case BrainType.PathFindMelee:
                    return new PathFindMelee(dungeon, models, body, view);
                default: throw new Exception("未定義のBrainTypeが指定された。");
            }
        }

        private void RemoveEnemy(IActorBrain brain, IActor enemy, IActorView view)
        {
            enemies.Remove(brain);
            models.RemoveEnemy(enemy);
            views.Remove(view);
        }
    }
}