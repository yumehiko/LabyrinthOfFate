﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace yumehiko.LOF.View
{
    /// <summary>
    /// Actorのアニメーションやビジュアルを管理。
    /// 画面上のポジションも管理。
    /// </summary>
    public class ActorView : MonoBehaviour, IActorView
    {
        private EffectController effectController;

        public async UniTask InvokeAnimation(Vector2Int point, float speedFactor, CancellationToken token)
        {
            await effectController.DoBuffEffect(point, speedFactor, token);
        }

        public async UniTask AttackAnimation(Vector2Int point, float speedFactor, CancellationToken token)
        {
            speedFactor *= 0.4f;
            Vector2 initPosition = transform.position;
            transform.localScale = new Vector3(1.0f, 1.5f, 1.0f);
            transform.position = (Vector2)point;
            Sequence sequence = DOTween.Sequence()
                .Append(transform.DOScaleY(1.0f, speedFactor))
                .Insert(0, transform.DOMove(initPosition, speedFactor))
                .SetLink(gameObject);
            await sequence.Play().ToUniTask(TweenCancelBehaviour.Complete, token);
        }

        public async UniTask ItemAnimation(Vector2Int point, float speedFactor, CancellationToken token)
        {
            speedFactor *= 0.4f;
            transform.localScale = new Vector3(1.0f, 1.5f, 1.0f);
            await transform.DOScaleY(1.0f, speedFactor)
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.Complete, token);
        }

        public async UniTask StepAnimation(Vector2Int point, float speedFactor, CancellationToken token)
        {
            speedFactor *= 0.4f;
            transform.localScale = new Vector3(1.0f, 0.75f, 1.0f);
            Sequence sequence = DOTween.Sequence()
                .Append(transform.DOScaleY(1.25f, 0.25f * speedFactor))
                .Append(transform.DOScaleY(1.0f, 0.75f * speedFactor))
                .Insert(0, transform.DOMove((Vector2)point, speedFactor))
                .SetLink(gameObject);
            await sequence.Play().ToUniTask(TweenCancelBehaviour.Complete, token);
        }

        public async UniTask WaitAnimation(Vector2Int point, float speedFactor, CancellationToken token)
        {
            speedFactor *= 0.4f;
            transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            await transform.DOScaleY(1.0f, speedFactor)
                .SetLink(gameObject)
                .ToUniTask(TweenCancelBehaviour.Complete, token);
        }

        public void Initialize(EffectController effectController)
        {
            this.effectController = effectController;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void WarpTo(Vector2Int position)
        {
            transform.position = (Vector2)position;
        }
    }
}