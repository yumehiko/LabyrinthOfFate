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

        private readonly ReactiveProperty<Vector2Int> position = new ReactiveProperty<Vector2Int>();
        private Sequence cursorFadeSequence;

        [Inject]
        public void Construct()
        {
            ReactiveInput.OnPointer
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
            var pointToVInt = new Vector2Int((int)(point.x + 0.5f), (int)(point.y + 0.5f));
            position.Value = pointToVInt;
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


    }
}