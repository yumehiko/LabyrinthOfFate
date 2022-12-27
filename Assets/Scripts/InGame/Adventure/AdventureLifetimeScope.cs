using VContainer;
using VContainer.Unity;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using yumehiko.LOF.Presenter;
using System.Collections;
using System.Collections.Generic;


namespace yumehiko.LOF
{
    public class AdventureLifetimeScope : LifetimeScope
    {
        [SerializeField] private AdventureProgress adventureProgress;
        [Space(10)]
        [SerializeField] private DungeonView dungeonView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(dungeonView);
            builder.RegisterComponent(adventureProgress);
            builder.RegisterEntryPoint<Adventure>(Lifetime.Singleton);
        }
    }
}