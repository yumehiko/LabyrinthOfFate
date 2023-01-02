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