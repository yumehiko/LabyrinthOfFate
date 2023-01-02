using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace yumehiko.LOF.View
{
    public class BuffEffect : MonoBehaviour
    {
        public float Duration => duration;

        [SerializeField] private float duration;
        [SerializeField] private ParticleSystem particle;

        public async UniTask DoEffect(Vector2Int position, float speedFactor, Transform parent, CancellationToken token)
        {
            var buffEffect = Instantiate(this, (Vector2)position, Quaternion.identity, parent);
            await UniTask.DelayFrame(3, cancellationToken: token);
            Destroy(buffEffect);
        }
    }
}