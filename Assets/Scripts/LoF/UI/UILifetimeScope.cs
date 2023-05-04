using LoF.UI.Info;
using LoF.UI.ResultMessage;
using LoF.UI.Reward;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LoF.UI
{
    //TODO:UI全部詰め込んでるけど分離した方がいいだろう。それぞれは無関係だし。
    public class UILifetimeScope : LifetimeScope
    {
        [SerializeField] private RewardsUI rewardsUI;
        [SerializeField] private InfoUI infoUI;
        [SerializeField] private RewardsUIView rewardsUIView;
        [SerializeField] private ResultMessageUIView resultMessageUIView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(infoUI);

            builder.RegisterComponent(rewardsUIView);
            builder.RegisterComponent(rewardsUI);

            builder.RegisterComponent(resultMessageUIView);
            builder.RegisterEntryPoint<ResultMessageUI>();
        }
    }
}