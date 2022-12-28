using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.Input;
using UniRx;
using DG.Tweening;
using VContainer;

namespace yumehiko.LOF.View
{
    public class UICursor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cursorRenderer;

        public IReadOnlyReactiveProperty<Vector2Int> Position => position;

        private bool isActive = true;
        private readonly ReactiveProperty<Vector2Int> position = new ReactiveProperty<Vector2Int>();
        private Sequence cursorFadeSequence;

        private void Awake()
        {
            _ = ReactiveInput.OnPointer
                .Where(_ => isActive)
                .Subscribe(point => OnMovePointer(point))
                .AddTo(this);

            position
                .Subscribe(position =>
                {
                    transform.position = (Vector2)position;
                    ActiveRenderer();
                }).AddTo(this);
        }

        private void OnMovePointer(Vector2 point)
        {
            var x = Mathf.RoundToInt(point.x);
            var y = Mathf.RoundToInt(point.y);
            position.Value = new Vector2Int(x, y);
        }

        private void ActiveRenderer()
        {
            if (cursorFadeSequence.IsActive())
            {
                cursorFadeSequence.Kill();
            }

            cursorRenderer.color = Color.white;
            cursorFadeSequence = DOTween.Sequence()
                .AppendInterval(1.0f)
                .Append(cursorRenderer.DOFade(0.0f, 0.75f))
                .SetLink(gameObject);
            _ = cursorFadeSequence.Play();
        }

        public void SetEnable(bool isEnable)
        {
            isActive = isEnable;
            if(isActive)
            {
                return;
            }
            if (cursorFadeSequence.IsActive())
            {
                cursorFadeSequence.Complete();
            }
        }
    }
}