using LoF.GameLogic.Dungeon;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LoF.GameLogic.Session
{
    public class AdventureLifetimeScope : LifetimeScope
    {
        [SerializeField] private AdventureProgress adventureProgress;

        [Space(10)] [SerializeField] private DungeonView dungeonView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(dungeonView);
            builder.RegisterComponent(adventureProgress);
            builder.RegisterEntryPoint<Adventure>();
        }
    }
}