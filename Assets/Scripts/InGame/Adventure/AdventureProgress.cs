using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using yumehiko.LOF.Model;
using UniRx;
using System;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// 冒険の進行度を管理。
    /// </summary>
    public class AdventureProgress : MonoBehaviour
    {
        public PlayerProfile PlayerProfile => playerProfile;

        [SerializeField] TextAsset firstDungeonJson;
        [SerializeField] List<ActorProfile> firstEnemys;
        [SerializeField] private PlayerProfile playerProfile;

        [SerializeField] private int floorCount = 0; //とりあえず見えるように
        private LevelAsset firstLevel;

        [Inject]
        public void Construct()
        {
            var firstDungeon = JsonUtility.FromJson<DungeonAsset>(firstDungeonJson.ToString());
            firstLevel = new LevelAsset(firstDungeon, firstEnemys);
        }

        public IReadOnlyList<ICardProfile> PickRewards()
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