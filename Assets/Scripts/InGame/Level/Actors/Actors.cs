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
    public class Actors : IDisposable
    {
        public Player Player { get; private set; }
        public IReadOnlyList<IActorBrain> Enemies => enemies;
        public IObservable<Unit> OnSetLevelActors => onSetLevelActors;

        private readonly Subject<Unit> onSetLevelActors = new Subject<Unit>();
        private readonly List<IActorBrain> enemies = new List<IActorBrain>();
        private readonly Transform viewParent;
        private readonly EffectController effectController;
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        [Inject]
        public Actors(Transform viewParent, EffectController effectController)
        {
            this.viewParent = viewParent;
            this.effectController = effectController;
        }

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
            foreach (var spawnPoint in spawnPoints)
            {
                SpawnActor(spawnPoint, level);
            }
            onSetLevelActors.OnNext(Unit.Default);
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

        /// <summary>
        /// プレイヤーのポジションを返す。
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetPlayerPosition()
        {
            return Player.Model.Position;
        }

        /// <summary>
        /// 指定地点にActorがいるか返す（プレイヤーも含む）。
        /// </summary>
        /// <param name="position"></param>
        /// <returns>いない場合はnull、いる場合はActorを返す。</returns>
        public IActorModel GetActorAt(Vector2Int position)
        {
            if (Player.Model.Position == position)
            {
                return Player.Model;
            }
            return GetEnemyAt(position);
        }

        /// <summary>
        /// 指定地点にEnemyがいるか返す。
        /// </summary>
        /// <param name="position"></param>
        /// <returns>いない場合はnull、いる場合はEnemyを返す。</returns>
		public IActorModel GetEnemyAt(Vector2Int position)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.Model.Position == position)
                {
                    return enemy.Model;
                }
            }

            return null;
        }

        /// <summary>
        /// [Debug]全ての敵を排除する。
        /// </summary>
        public void DefeatAllEnemy()
        {
            var temp = new List<IActorBrain>(enemies);
            foreach(var enemy in temp)
            {
                RemoveEnemy(enemy);
            }
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
            var id = UnityEngine.Random.Range(0, level.EnemyProfiles.Count);
            var profile = level.EnemyProfiles[id];
            var model = new ActorModel(profile, spawnPoint.Position, ActorType.Enemy);
            var view = UnityEngine.Object.Instantiate(profile.View, (Vector2)spawnPoint.Position, Quaternion.identity, viewParent);
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