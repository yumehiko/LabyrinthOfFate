using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using System;

namespace yumehiko.LOF.View
{
    public class RewardsUIView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private List<RewardView> views;
        [SerializeField] private CommandButton ignoreButton;

        public IObservable<int> OnPick => onPick;
        public IObservable<Unit> OnIgnore => ignoreButton.OnClick;

        private readonly Subject<int> onPick = new Subject<int>();

        private void Awake()
        {
            for (int i = 0; i < 3; i++)
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
            for (int i = 0; i < 3; i++)
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
            for (int i = 0; i < 3; i++)
            {
                views[i].DisableView(0.3f, token).Forget();
            }
            await group.DOFade(0.0f, 0.5f).SetLink(gameObject).ToUniTask(cancellationToken: token);
        }

        public void SetRewardsInfo(IReadOnlyList<IItemView> itemViews)
        {
            for (int i = 0; i < 3; i++)
            {
                views[i].SetRewardInfo(itemViews[i]);
            }
        }
    }
}