using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LoF.GameLogic.Entity.Item;
using UniRx;
using UnityEngine;

namespace LoF.UI.Reward
{
    public class RewardsUIView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private List<RewardView> views;
        [SerializeField] private CommandButton ignoreButton;

        private readonly Subject<int> onPick = new();

        public IObservable<int> OnPick => onPick;
        public IObservable<Unit> OnIgnore => ignoreButton.OnClick;

        private void Awake()
        {
            for (var i = 0; i < 3; i++)
            {
                var id = i;
                views[i].OnClick
                    .Subscribe(_ => onPick.OnNext(id))
                    .AddTo(this);
            }
        }

        public async UniTask EnableUI(CancellationToken token)
        {
            await group.DOFade(0.95f, 0.5f).SetLink(gameObject).ToUniTask(cancellationToken: token);
            for (var i = 0; i < 3; i++)
            {
                views[i].EnableView(0.4f, token).Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: token);
            }

            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public async UniTask DisableUI(CancellationToken token)
        {
            group.interactable = false;
            group.blocksRaycasts = false;
            for (var i = 0; i < 3; i++) views[i].DisableView(0.3f, token).Forget();
            await group.DOFade(0.0f, 0.5f).SetLink(gameObject).ToUniTask(cancellationToken: token);
        }

        public void SetRewardsInfo(IReadOnlyList<IItemView> itemViews)
        {
            for (var i = 0; i < 3; i++) views[i].SetRewardInfo(itemViews[i]);
        }
    }
}