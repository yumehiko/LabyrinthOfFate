using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.UI;

namespace yumehiko.LOF.View
{
    public class RewardPickButton : MonoBehaviour
    {
        [SerializeField] private Image pickFrame;
        [SerializeField] private CanvasGroup group;
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
                .Subscribe(_ => Debug.Log("PickCard"))
                .AddTo(this);
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