using Cysharp.Threading.Tasks;
using LoF.GameLogic.Entity.Actor;
using UnityEngine;

namespace LoF.Effect
{
    public class BuffEffect : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private ParticleSystem particle;
        public float Duration => duration;

        public async UniTask DoEffect(Vector2Int position, Transform parent, ActRequest request)
        {
            var buffEffect = Instantiate(this, (Vector2)position, Quaternion.identity, parent);
            await UniTask.DelayFrame(3, cancellationToken: request.AnimationCT);
            Destroy(buffEffect);
        }
    }
}