using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// Actorの実体のコレクション。
    /// TODO:多分なくせる。ややこしい。ActorのコレクションはActorPresenterにまかせて、個々のmodelとviewはactor本体が持つだけにする。
    /// </summary>
    public class ActorModels
    {
        public IActorModel Player => player;
        public IReadOnlyList<IActorModel> Enemies => enemies;

        private IActorModel player;
        private readonly List<IActorModel> enemies = new List<IActorModel>();

        public IActorModel SpawnPlayer(IActorProfile profile, Vector2Int position)
        {
            var body = new ActorModel(profile, position);
            player = body;
            return body;
        }

        public void AddPlayer(IActorModel player)
        {
            this.player = player;
        }

        public IActorModel SpawnEnemy(IActorProfile profile, Vector2Int position)
        {
            var enemy = new ActorModel(profile, position);
            enemies.Add(enemy);
            return enemy;
        }

        public void RemoveEnemy(IActorModel enemy)
        {
            enemies.Remove(enemy);
        }

        /// <summary>
        /// 指定したActorがプレイヤーかを返す。
        /// </summary>
        /// <returns></returns>
        public bool IsPlayer(IActorModel actor)
        {
            return actor == player;
        }

        /// <summary>
        /// プレイヤーのポジションを返す。
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetPlayerPosition()
        {
            return player.Position;
        }

        /// <summary>
        /// 指定地点にActorがいるか返す（プレイヤーも含む）。
        /// </summary>
        /// <param name="position"></param>
        /// <returns>いない場合はnull、いる場合はActorを返す。</returns>
        public IActorModel GetActorAt(Vector2Int position)
        {
            if (player.Position == position)
            {
                return player;
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
                if (enemy.Position == position)
                {
                    return enemy;
                }
            }

            return null;
        }
    }
}