using DG.Tweening;
using LoF.Input;
using UniRx;
using UnityEngine;

namespace LoF.UI.Info
{
    public class UICursor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cursorRenderer;
        private readonly ReactiveProperty<Vector2Int> position = new();
        private Sequence cursorFadeSequence;

        private bool isActive = true;

        public IReadOnlyReactiveProperty<Vector2Int> Position => position;

        private void Awake()
        {
            _ = ReactiveInput.OnPointer
                .Where(_ => isActive)
                .Subscribe(OnMovePointer)
                .AddTo(this);

            position
                .Subscribe(value =>
                {
                    transform.position = (Vector2)value;
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
            if (cursorFadeSequence.IsActive()) cursorFadeSequence.Kill();

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
            if (isActive) return;
            if (cursorFadeSequence.IsActive()) cursorFadeSequence.Complete();
        }
    }
}