using VContainer;
using VContainer.Unity;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using System.Collections;
using System.Collections.Generic;


namespace yumehiko.LOF.Presenter
{
    public class AdventureLifetimeScope : LifetimeScope
    {
        [SerializeField] private TextAsset dungeonJson;
        [SerializeField] private List<ActorProfile> enemyProfiles;
        [SerializeField] private PlayerProfile playerProfile;
        [Space(10)]
        [SerializeField] private DungeonView dungeonView;
        [SerializeField] private InfoUI infoUI;

        protected override void Configure(IContainerBuilder builder)
        {
            var dungeonAsset = JsonUtility.FromJson<DungeonAsset>(dungeonJson.ToString());
            builder.RegisterInstance(dungeonAsset);
            builder.RegisterComponent(playerProfile);
            builder.RegisterComponent(enemyProfiles);

            builder.RegisterComponent(dungeonView);
            builder.RegisterComponent(infoUI);

            builder.RegisterEntryPoint<Adventure>(Lifetime.Singleton);
        }
    }
}