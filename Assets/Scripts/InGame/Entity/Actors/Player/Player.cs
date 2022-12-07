using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using yumehiko.Input;

namespace yumehiko.LOF
{
    public class Player : MonoBehaviour, IActor
    {
        [SerializeField] private GridMovement gridMovement;
        [SerializeField] private ActorVisual visual;

        public EntityType EntityType => EntityType.Actor;
        public Affiliation Affiliation => Affiliation.Player;

        private PlayerControlMode controlMode = PlayerControlMode.Move;
        private readonly AsyncReactiveProperty<ActorDirection> inputDirection = new AsyncReactiveProperty<ActorDirection>(ActorDirection.None);

        private void Awake()
        {
            ReactiveInput.OnMove
                .Where(_ => controlMode == PlayerControlMode.Move)
                .Subscribe(value => inputDirection.Value = value)
                .AddTo(this);
        }

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public async UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token)
        {
            //移動入力・UI入力を待つ。
            var inputs = new List<UniTask>
            {
                InputMove(animationSpeedFactor, token),
            };

            //いずれかのターン消費行動が確定したら、行動終了。
            await UniTask.WhenAny(inputs);
        }

        private async UniTask InputMove(float animationSpeedFactor, CancellationToken token)
        {
            bool isMoveConfirm = false;
            ReactiveInput.ClearDirection();

            while (!isMoveConfirm)
            {
                var direction = await inputDirection.WaitAsync(token);

                //ここで実際に移動可能かをチェックする。不可能なら無視する。
                IEntity entity = gridMovement.CheckEntityTo(direction);
                var entityType = entity == null ? EntityType.None : entity.EntityType;
                if (entityType != EntityType.None)
                {
                    continue;
                }

                //移動可能なことが確定したので、移動する。
                var endPoint = gridMovement.StepTo(direction);
                await visual.StepAnimation(endPoint, animationSpeedFactor);
                isMoveConfirm = true;
            }
        }
    }
}