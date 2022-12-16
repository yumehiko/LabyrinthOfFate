using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace yumehiko.LOF.View
{
    public class ActorView : MonoBehaviour, IActorView
    {
        public async UniTask AttackAnimation(float animationSpeedFactor, CancellationToken token)
        {
            transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            await transform.DOScaleY(1.0f, animationSpeedFactor).ToUniTask(cancellationToken: token);
        }

        public async UniTask ItemAnimation(float animationSpeedFactor, CancellationToken token)
        {
            transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            await transform.DOScaleY(1.0f, animationSpeedFactor).ToUniTask(cancellationToken: token);
        }

        public async UniTask StepAnimation(float animationSpeedFactor, CancellationToken token)
        {
            transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            await transform.DOScaleY(1.0f, animationSpeedFactor).ToUniTask(cancellationToken: token);
        }

        public async UniTask WaitAnimation(float animationSpeedFactor, CancellationToken token)
        {
            await UniTask.DelayFrame(1);
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}