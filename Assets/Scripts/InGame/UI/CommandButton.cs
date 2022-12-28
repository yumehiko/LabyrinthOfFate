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
                .Subscribe(_ => Select())
                .AddTo(this);

            _ = button.OnPointerExitAsObservable()
                .Subscribe(_ => Deselect())
                .AddTo(this);
        }

        public void SetEnable()
        {
            graphic.SetAlpha(0.8f);
            button.interactable = true;
        }

        public void SetDisable()
        {
            graphic.SetAlpha(0.25f);
            button.interactable = false;
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