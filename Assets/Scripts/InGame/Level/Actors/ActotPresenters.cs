﻿using System.Collections;
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
    public class ActorPresenters : IDisposable
    {
        public Player Player { get; private set; }
        public IReadOnlyList<IActorBrain> Enemies => enemies;
        public ActorModels Models => models;
        public IObservable<Unit> OnDefeatAllEnemy => onDefeatAllEnemy;

        private readonly Subject<Unit> onDefeatAllEnemy = new Subject<Unit>();
        private readonly List<IActorBrain> enemies = new List<IActorBrain>();
        private readonly ActorViews views;
        private readonly ActorModels models;
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        [Inject]
        public ActorPresenters(ActorModels models, ActorViews views)
        {
            this.models = models;
            this.views = views;
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        public void SpawnActors(ActorSpawnPoints spawnPoints, Level level)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                SpawnActor(spawnPoint, level);
            }
        }

        public void ClearActorsWithoutPlayer()
        {
            var deletableList = new List<IActorBrain>(enemies);
            foreach (var enemy in deletableList)
            {
                RemoveEnemy(enemy);
            }
            enemies.Clear();
        }

        private void SpawnActor(ActorSpawnPoint spawnPoint, Level level)
        {
            switch (spawnPoint.Type)
            {
                case ActorType.Player:
                    SetPlayerPosition(spawnPoint);
                    return;
                case ActorType.Enemy:
                    SpawnEnemy(spawnPoint, level);
                    return;
            }
        }

        public void AddPlayer(Player player, IActor model, IActorView view)
        {
            models.AddPlayer(model);
            views.AddView(view);
            Player = player;
            disposables.Add(player);
        }

        private void SetPlayerPosition(ActorSpawnPoint spawnPoint)
        {
            Player.WarpTo(spawnPoint.Position);
        }

        private IActorBrain SpawnEnemy(ActorSpawnPoint spawnPoint, Level level)
        {
            var id = UnityEngine.Random.Range(0, level.EnemyProfiles.Count);
            var profile = level.EnemyProfiles[id];
            var model = models.SpawnEnemy(profile, spawnPoint.Position);
            var view = views.SpawnActorView(spawnPoint.Position, profile.View);
            var brain = SpawnBrain(profile.BrainType, model, view, level);
            enemies.Add(brain);
            model.IsDied
                .Where(isTrue => isTrue)
                .First()
                .Subscribe(_ => DefeatEnemy(brain))
                .AddTo(disposables);
            return brain;
        }

        private IActorBrain SpawnBrain(BrainType type, IActor body, IActorView view, Level level)
        {
            switch (type)
            {
                case BrainType.RandomStep:
                    return new RandomStepper(level, body, view);
                case BrainType.PathFindMelee:
                    return new PathFindMelee(level, body, view);
                default: throw new Exception("未定義のBrainTypeが指定された。");
            }
        }

        private void DefeatEnemy(IActorBrain brain)
        {
            RemoveEnemy(brain);
            if (enemies.Count == 0)
            {
                onDefeatAllEnemy.OnNext(Unit.Default);
            }
        }

        private void RemoveEnemy(IActorBrain brain)
        {
            enemies.Remove(brain);
            models.RemoveEnemy(brain.Model);
            views.Remove(brain.View);
        }
    }
}