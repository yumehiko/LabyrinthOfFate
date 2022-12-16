using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace yumehiko.LOF.Model
{
    public class Entities
    {
        public IActor Player => player;
        public IReadOnlyList<IActor> Enemies => enemies;

        private IActor player;
        private readonly List<IActor> enemies = new List<IActor>();

        [Inject]
        public Entities()
        {
            Debug.Log("Entities");
        }

        public ActorBody SpawnPlayer(ActorStatus status, Vector2Int position)
        {
            var body = new ActorBody(status, position);
            player = body;
            return body;
        }

        public ActorBody SpawnEnemy(ActorStatus status, Vector2Int position)
        {
            var enemy = new ActorBody(status, position);
            enemies.Add(enemy);
            return enemy;
        }

        public void RemoveEnemy(IActor enemy)
        {
            enemies.Remove(enemy);
        }

        /// <summary>
        /// 指定地点にPlayerがいるならPlayerを返す。
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public IActor GetPlayerAt(Vector2Int position)
        {
            if (player.Position == position)
            {
                return player;
            }
            return null;
        }

        /// <summary>
        /// 指定地点にActorがいるか返す（プレイヤーも含む）。
        /// </summary>
        /// <param name="position"></param>
        /// <returns>いない場合はnull、いる場合はActorを返す。</returns>
        public IActor GetActorAt(Vector2Int position)
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
		public IActor GetEnemyAt(Vector2Int position)
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