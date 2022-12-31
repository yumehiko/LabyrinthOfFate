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
    //TODO:UI全部詰め込んでるけど分離した方がいいだろう。それぞれは無関係だし。
    public class UILifetimeScope : LifetimeScope
    {
        [SerializeField] private Rewards rewards;
        [SerializeField] private InfoUI infoUI;
        [SerializeField] private RewardsUI rewardsUI;
        [SerializeField] private InventoryUIView inventoryUIView;
        [SerializeField] private ResultMessageUIView resultMessageUIView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(inventoryUIView);
            builder.Register<InventoryUI>(Lifetime.Singleton);

            builder.RegisterComponent(infoUI);

            builder.RegisterComponent(rewardsUI);
            builder.RegisterComponent(rewards);

            builder.RegisterComponent(resultMessageUIView);
            builder.RegisterEntryPoint<ResultMessageUI>(Lifetime.Singleton);
        }
    }
}