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
        [SerializeField] private AdventureProgress adventureProgress;
        [Space(10)]
        [SerializeField] private DungeonView dungeonView;
        [SerializeField] private InfoUI infoUI;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(dungeonView);
            builder.RegisterComponent(infoUI);

            builder.RegisterComponent(adventureProgress);
            builder.RegisterEntryPoint<Adventure>(Lifetime.Singleton);
        }
    }
}