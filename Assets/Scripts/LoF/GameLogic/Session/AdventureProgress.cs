using System.Collections.Generic;
using LoF.GameLogic.Dungeon.Material;
using LoF.GameLogic.Entity.Actor;
using LoF.GameLogic.Entity.Item.Card;
using UnityEngine;
using VContainer;

namespace LoF.GameLogic.Session
{
    /// <summary>
    ///     冒険の進行度を管理。
    /// </summary>
    public class AdventureProgress : MonoBehaviour
    {
        [SerializeField] private TextAsset firstDungeonJson;
        [SerializeField] private List<ActorProfile> firstEnemys;
        [SerializeField] private PlayerProfile playerProfile;

        [SerializeField] private int floorCount; //とりあえず見えるように
        private LevelAsset firstLevel;
        public PlayerProfile PlayerProfile => playerProfile;

        [Inject]
        public void Construct()
        {
            var firstDungeon = JsonUtility.FromJson<DungeonAsset>(firstDungeonJson.ToString());
            firstLevel = new LevelAsset(firstDungeon, firstEnemys);
        }

        public IReadOnlyList<ICardModel> PickRewards()
        {
            return null;
        }

        public LevelAsset PickNextLevelAssets()
        {
            floorCount++;
            return firstLevel;
        }
    }
}