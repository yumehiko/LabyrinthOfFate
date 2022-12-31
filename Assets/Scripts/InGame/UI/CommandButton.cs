using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using UniRx.Triggers;

namespace yumehiko.LOF.View
{
    public class CommandButton : MonoBehaviour
    {
        [SerializeField] private Graphic graphic;
        [SerializeField] private Button button;

        public IObservable<Unit> OnClick => button.OnClickAsObservable();

        private void Awake()
        {
            _ = button.OnPointerEnterAsObservable()
                .Where(_ => button.interactable)
                .Subscribe(_ => Select())
                .AddTo(this);

            _ = button.OnPointerExitAsObservable()
                .Where(_ => button.interactable)
                .Subscribe(_ => Deselect())
                .AddTo(this);
        }

        public void SetEnable(bool isEnable)
        {
            float alpha = isEnable ? 0.8f : 0.25f;
            graphic.SetAlpha(alpha);
            button.interactable = isEnable;
        }

        private void Select()
        {
            graphic.SetAlpha(1.0f);
        }

        private void Deselect()
        {
            graphic.SetAlpha(0.8f);
        }
    }
}