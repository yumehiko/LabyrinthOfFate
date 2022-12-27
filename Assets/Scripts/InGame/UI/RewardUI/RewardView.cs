using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.LOF.View
{
    public class RewardView : MonoBehaviour
    {
        [SerializeField] private Image pickFrame;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI stats;
        [SerializeField] private TextMeshProUGUI invokeEffect;
        [SerializeField] private Image cardFrame;

        public System.IObservable<Unit> OnClick => onClick;

        private readonly Subject<Unit> onClick = new Subject<Unit>();
        private Tweener focusTweener;

        private void Awake()
        {
            _ = pickFrame.OnPointerEnterAsObservable()
                .Subscribe(_ => CursorEnterButton())
                .AddTo(this);

            _ = pickFrame.OnPointerExitAsObservable()
                .Subscribe(_ => CursorExitButton())
                .AddTo(this);

            _ = pickFrame.OnPointerDownAsObservable()
                .Subscribe(_ => onClick.OnNext(Unit.Default))
                .AddTo(this);
        }

        public async UniTask EnableView(float duration, CancellationToken token)
        {
            group.alpha = 0.0f;
            await group.DOFade(0.5f, duration).SetLink(gameObject).ToUniTask(cancellationToken: token);
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public async UniTask DisableView(float duration, CancellationToken token)
        {
            group.interactable = false;
            group.blocksRaycasts = false;
            group.alpha = 0.5f;
            await group.DOFade(0.0f, duration).SetLink(gameObject).ToUniTask(cancellationToken: token);
        }

        public void SetRewardInfo(IItemView itemView)
        {
            cardName.text = itemView.Name;
            stats.text = itemView.StatsInfo;
            invokeEffect.text = itemView.InvokeEffect;
            cardFrame.sprite = itemView.Frame;
        }

        private void CursorEnterButton()
        {
            focusTweener?.Kill();
            focusTweener = group.DOFade(1.0f, 0.2f).SetLink(gameObject);
        }

        private void CursorExitButton()
        {
            focusTweener?.Kill();
            focusTweener = group.DOFade(0.5f, 0.2f).SetLink(gameObject);
        }
    }
}