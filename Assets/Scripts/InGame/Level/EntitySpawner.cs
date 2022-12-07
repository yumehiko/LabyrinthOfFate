using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    /// <summary>
    /// Entityを生成する。
    /// </summary>
	public class EntitySpawner : MonoBehaviour
	{
        [SerializeField] private Transform spawnPointsParent;
        [SerializeField] private Transform entitiesParent;
        [SerializeField] private Transform entitiesVisualParent;
        [SerializeField] private PlayerInformation playerInformation;
        [SerializeField] private List<ActorProfile> spawnableEnemyProfiles;

        public IActor Player => player;
        public IReadOnlyList<IActor> Enemies => enemies;

        private IActor player;
        private readonly List<IActor> enemies = new List<IActor>();

        public void SpawnEntities()
        {
            SpawnActors();
        }

        /// <summary>
        /// 全てのActorを生成する。
        /// </summary>
        private void SpawnActors()
        {
            var spawnPoints = spawnPointsParent.GetComponentsInChildren<ISpawnPoint>();
            foreach (ISpawnPoint spawnPoint in spawnPoints)
            {
                switch (spawnPoint.Affiliation)
                {
                    case Affiliation.Player:
                        player = SpawnPlayer(spawnPoint);
                        break;
                    case Affiliation.Enemy:
                        var id = Random.Range(0, spawnableEnemyProfiles.Count);
                        enemies.Add(SpawnActor(spawnPoint, spawnableEnemyProfiles[id]));
                        break;
                    default:
                        throw new System.Exception("未定義の陣営が生成された。");
                }
            }
        }

        /// <summary>
        /// Actorを生成する。
        /// </summary>
        private IActor SpawnActor(ISpawnPoint spawnPoint, ActorProfile profile)
        {
            var actor = Instantiate(profile.Brain, spawnPoint.Position, Quaternion.identity, entitiesParent);
            var visual = Instantiate(profile.Visual, spawnPoint.Position, Quaternion.identity, entitiesVisualParent);
            actor.SetProfile(profile.Status, visual);
            return actor;
        }

        /// <summary>
        /// プレイヤーが操作するキャラクターを生成する。
        /// </summary>
        private IActor SpawnPlayer(ISpawnPoint spawnPoint)
        {
            var playerPrefab = playerInformation.PlayerPrefab;
            var visualPrefab = playerInformation.PlayerVisualPrefab;
            var player = Instantiate(playerPrefab, spawnPoint.Position, Quaternion.identity, entitiesParent);
            var visual = Instantiate(visualPrefab, spawnPoint.Position, Quaternion.identity, entitiesVisualParent);
            player.SetProfile(playerInformation.PlayerStatus, visual);
            return player;
        }
    }
}