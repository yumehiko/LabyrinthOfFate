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
        UniTask InvokeAnimation(Vector2Int point, ActRequest request);
        UniTask WaitAnimation(Vector2Int point, ActRequest request);
        UniTask StepAnimation(Vector2Int point, ActRequest request);
        UniTask AttackAnimation(Vector2Int point, ActRequest request);
        UniTask ItemAnimation(Vector2Int point, ActRequest request);
        void WarpTo(Vector2Int position);
    }
}