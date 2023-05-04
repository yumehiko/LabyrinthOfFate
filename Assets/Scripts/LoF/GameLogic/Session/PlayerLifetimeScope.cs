using LoF.GameLogic.Entity.Actor.Brains;
using LoF.UI.Inventory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LoF.GameLogic.Session
{
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerProfile playerProfile;
        [SerializeField] private InventoryUIView inventoryUIView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(inventoryUIView);
            builder.Register<InventoryUI>(Lifetime.Singleton);

            builder.RegisterComponent(playerProfile);
            builder.Register<Player>(Lifetime.Singleton);
        }
    }
}