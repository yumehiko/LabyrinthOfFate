using System;
using System.Collections.Generic;
using LoF.Effect;
using LoF.GameLogic.Dungeon.Material;
using LoF.GameLogic.Entity.Actor.Brains;
using LoF.GameLogic.Entity.Actor.Model;
using UniRx;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace LoF.GameLogic.Entity.Actor
{
    /// <summary>
    ///     実行中のゲームの、全てのActorBrainのコレクション。
    /// </summary>
    public class Actors : IDisposable
    {
        private readonly CompositeDisposable disposables = new();
        private readonly EffectController effectController;
        private readonly List<IActorBrain> enemies = new();

        private readonly Subject<Unit> setLevelActors = new();
        private readonly Transform viewParent;

        [Inject]
        public Actors(Transform viewParent, EffectController effectController)
        {
            this.viewParent = viewParent;
            this.effectController = effectController;
        }

        public Player Player { get; private set; }
        public IReadOnlyList<IActorBrain> Enemies => enemies;
        public IObservable<Unit> OnSetLevelActors => setLevelActors;

        public void Dispose()
        {
            disposables.Dispose();
        }

        public void AddPlayer(Player player, IActorModel model, IActorView view)
        {
            Player = player;
            disposables.Add(player);
        }

        public void SpawnActors(ActorSpawnPoints spawnPoints, Level level)
        {
            foreach (var spawnPoint in spawnPoints) SpawnActor(spawnPoint, level);
            setLevelActors.OnNext(Unit.Default);
        }

        public void ClearActorsWithoutPlayer()
        {
            var deletableList = new List<IActorBrain>(enemies);
            foreach (var enemy in deletableList) RemoveEnemy(enemy);
            enemies.Clear();
        }

        /// <summary>
        ///     プレイヤーのポジションを返す。
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetPlayerPosition()
        {
            return Player.Model.Position;
        }

        /// <summary>
        ///     指定地点にActorがいるか返す（プレイヤーも含む）。
        /// </summary>
        /// <param name="position"></param>
        /// <returns>いない場合はnull、いる場合はActorを返す。</returns>
        public IActorModel GetActorAt(Vector2Int position)
        {
            if (Player.Model.Position == position) return Player.Model;
            return GetEnemyAt(position);
        }

        /// <summary>
        ///     指定地点にEnemyがいるか返す。
        /// </summary>
        /// <param name="position"></param>
        /// <returns>いない場合はnull、いる場合はEnemyを返す。</returns>
        public IActorModel GetEnemyAt(Vector2Int position)
        {
            foreach (var enemy in enemies)
                if (enemy.Model.Position == position)
                    return enemy.Model;

            return null;
        }

        /// <summary>
        ///     [Debug]全ての敵を排除する。
        /// </summary>
        public void DefeatAllEnemy()
        {
            var temp = new List<IActorBrain>(enemies);
            foreach (var enemy in temp) RemoveEnemy(enemy);
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

        private void SetPlayerPosition(ActorSpawnPoint spawnPoint)
        {
            Player.WarpTo(spawnPoint.Position);
        }

        private IActorBrain SpawnEnemy(ActorSpawnPoint spawnPoint, Level level)
        {
            var id = Random.Range(0, level.EnemyProfiles.Count);
            var profile = level.EnemyProfiles[id];
            var model = new ActorModel(profile, spawnPoint.Position, ActorType.Enemy);
            var view = Object.Instantiate(profile.View, (Vector2)spawnPoint.Position, Quaternion.identity, viewParent);
            view.Initialize(effectController);
            var brain = SpawnBrain(profile.BrainType, model, view, level);
            enemies.Add(brain);
            model.IsDied
                .Where(isTrue => isTrue)
                .First()
                .Subscribe(_ => RemoveEnemy(brain))
                .AddTo(disposables);
            return brain;
        }

        private IActorBrain SpawnBrain(BrainType type, IActorModel body, IActorView view, Level level)
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

        private void RemoveEnemy(IActorBrain brain)
        {
            enemies.Remove(brain);
            brain.Destroy();
        }
    }
}