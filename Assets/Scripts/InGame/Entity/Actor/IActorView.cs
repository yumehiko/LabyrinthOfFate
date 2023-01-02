using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.LOF.View
{
    public interface IActorView
    {
        void Initialize(EffectController effectController);
        void DestroySelf();
        UniTask InvokeAnimation(Vector2Int point, float speedFactor, CancellationToken token);
        UniTask WaitAnimation(Vector2Int point, float speedFactor, CancellationToken token);
        UniTask StepAnimation(Vector2Int point, float speedFactor, CancellationToken token);
        UniTask AttackAnimation(Vector2Int point, float speedFactor, CancellationToken token);
        UniTask ItemAnimation(Vector2Int point, float speedFactor, CancellationToken token);
        void WarpTo(Vector2Int position);
    }
}