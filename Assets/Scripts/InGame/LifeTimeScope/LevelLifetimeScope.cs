using VContainer;
using VContainer.Unity;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using yumehiko.LOF.Presenter;
using System.Collections;
using System.Collections.Generic;

public class LevelLifetimeScope : LifetimeScope
{
    [SerializeField] private TextAsset dungeonJson; //TODO: ここではなくゲーム管理側が渡す。
    [SerializeField] private List<ActorProfile> enemyProfiles; //TODO: ここではなくゲーム管理側が渡す。
    [SerializeField] private PlayerProfile playerInformation; //TODO: ここではなくゲーム管理側が渡す。
    [Space(10)]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private DungeonView dungeonView;
    [SerializeField] private ActorViews actorViews;

    protected override void Configure(IContainerBuilder builder)
    {
        var dungeonAsset = JsonUtility.FromJson<DungeonAsset>(dungeonJson.ToString());
        dungeonAsset.Dungeon.MakePathFinder();
        builder.RegisterInstance(dungeonAsset.ActorSpawnPoints);
        builder.RegisterInstance(dungeonAsset.Dungeon);
        builder.RegisterInstance(playerInformation);
        builder.RegisterInstance(enemyProfiles);

        builder.RegisterComponent(actorViews);
        builder.Register<Actors>(Lifetime.Singleton);
        builder.Register<ActorBrains>(Lifetime.Singleton);

        builder.RegisterComponent(mainCamera);
        builder.RegisterComponent(dungeonView);
        builder.Register<Turn>(Lifetime.Singleton);
        builder.RegisterEntryPoint<Level>(Lifetime.Singleton);
    }
}
