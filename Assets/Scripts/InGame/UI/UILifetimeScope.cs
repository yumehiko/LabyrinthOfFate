using VContainer;
using VContainer.Unity;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;
using yumehiko.LOF.Presenter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    public class UILifetimeScope : LifetimeScope
    {
        [SerializeField] private Rewards rewards;
        [SerializeField] private InfoUI infoUI;
        [SerializeField] private RewardsUI rewardsUI;
        [SerializeField] private InventoryUI inventoryUI;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InventoryModel>(Lifetime.Singleton);
            builder.Register<Inventory>(Lifetime.Singleton);
            builder.RegisterComponent(inventoryUI);

            builder.RegisterComponent(infoUI);

            builder.RegisterComponent(rewards);
            builder.RegisterComponent(rewardsUI);
        }
    }
}