using VContainer;
using VContainer.Unity;
using UnityEngine;
using yumehiko.LOF.View;
using yumehiko.LOF.Presenter;

public class InLevelInfoLifetimeScope : LifetimeScope
{
    [SerializeField] private InfoUI infoUI;
    [SerializeField] private UICursor cursor;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(cursor);
        builder.RegisterComponent(infoUI);
    }
}
