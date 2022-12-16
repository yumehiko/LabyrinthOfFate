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
    /// 実行中のゲームの、全てのエンティティを生成し、そのPresenter全てを保持する。
    /// </summary>
    public class EntitySpawner : IDisposable
    {
        public IActorBrain Player => player;
        public IReadOnlyList<IActorBrain> Enemies => enemies;

        private IActorBrain player;
        private readonly List<IActorBrain> enemies;
        private readonly EntityViews views;
        private readonly Entities models;

        private readonly List<ActorProfile> enemyProfiles;
        private readonly Floor floor;
        private readonly PlayerInformation playerInformation;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        [Inject]
        public EntitySpawner(
            Dungeon dungeon,
            PlayerInformation playerInformation,
            List<ActorProfile> enemyProfiles,
            Entities models,
            EntityViews views)
        {
            Debug.Log("SpawnerSpawn");
            this.models = models;
            this.views = views;
            this.floor = dungeon.Floor;
            this.playerInformation = playerInformation;
            this.enemyProfiles = enemyProfiles;
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        public void SpawnEntities(EntitySpawnPoints spawnPoints)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                Debug.Log($"{spawnPoint.Type}");
                SpawnEntity(spawnPoint);
            }
            if (player == null)
            {
                throw new Exception("Playerが生成できなかった。");
            }
        }

        private void SpawnEntity(EntitySpawnPoint spawnPoint)
        {
            switch (spawnPoint.Type)
            {
                case ActorType.Player:
                    if (player != null)
                    {
                        return;
                    }
                    player = SpawnPlayer(spawnPoint);
                    return;
                case ActorType.Enemy:
                    SpawnEnemy(spawnPoint);
                    return;
            }
        }

        private IActorBrain SpawnPlayer(EntitySpawnPoint spawnPoint)
        {
            var body = models.SpawnPlayer(playerInformation.Status, spawnPoint.Position);
            var view = views.SpawnEntityView(spawnPoint, playerInformation.View);
            var brain = new Player(floor, models, body, view);
            return brain;

        }

        private IActorBrain SpawnEnemy(EntitySpawnPoint spawnPoint)
        {
            var id = UnityEngine.Random.Range(0, enemyProfiles.Count);
            var profile = enemyProfiles[id];
            switch (profile.Status.BrainType)
            {
                case BrainType.RandomStep:
                    var body = models.SpawnEnemy(profile.Status, spawnPoint.Position);
                    var view = views.SpawnEntityView(spawnPoint, profile.View);
                    var brain = new RandomStepper(floor, models, body, view);
                    body.OnDie.Subscribe(_ => RemoveEnemy(brain, body, view)).AddTo(disposables);
                    return brain;
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