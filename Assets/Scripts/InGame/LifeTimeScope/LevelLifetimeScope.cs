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
    [SerializeField] private PlayerInformation playerInformation; //TODO: ここではなくゲーム管理側が渡す。
    [Space(10)]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private DungeonView dungeonView;
    [SerializeField] private EntityViews entityViews;

    protected override void Configure(IContainerBuilder builder)
    {
        var dungeon = JsonUtility.FromJson<Dungeon>(dungeonJson.ToString());
        builder.RegisterInstance(dungeon);
        builder.RegisterInstance(playerInformation);
        builder.RegisterInstance(enemyProfiles);

        builder.RegisterComponent(entityViews);
        builder.Register<Entities>(Lifetime.Singleton);
        builder.Register<EntityPresenters>(Lifetime.Singleton);

        builder.RegisterComponent(mainCamera);
        builder.RegisterComponent(dungeonView);
        builder.Register<Turn>(Lifetime.Singleton);
        builder.RegisterEntryPoint<Level>(Lifetime.Singleton);
    }
}
