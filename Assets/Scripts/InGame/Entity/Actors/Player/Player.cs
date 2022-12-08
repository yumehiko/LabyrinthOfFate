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
        public EntityType EntityType => EntityType.Actor;
        public Affiliation Affiliation => Affiliation.Player;
        public ActorBody Body => body;

        [SerializeField] private GridMovement gridMovement;

        private ActorVisual visual;
        private ActorBody body;
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
                InputDirection(animationSpeedFactor, token),
            };

            //いずれかのターン消費行動が確定したら、行動終了。
            await UniTask.WhenAny(inputs);
        }

        public void SetProfile(ActorStatus status, ActorVisual visual)
        {
            if(body != null)
            {
                return;
            }
            body = new ActorBody(status);
            this.visual = visual;
        }

        /// <summary>
        /// 方向入力による行動。
        /// </summary>
        /// <param name="animationSpeedFactor"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask InputDirection(float animationSpeedFactor, CancellationToken token)
        {
            bool isActionComplete = false;
            ReactiveInput.ClearDirection();

            while (!isActionComplete)
            {
                var direction = await inputDirection.WaitAsync(token);

                //ここで実際に移動可能かをチェックする。不可能なら無視する。
                IEntity entity = gridMovement.CheckEntityTo(direction);
                var entityType = entity == null ? EntityType.None : entity.EntityType;

                switch(entityType)
                {
                    case EntityType.None:
                        await Move(direction, animationSpeedFactor);
                        break;
                    case EntityType.Actor:
                        var actor = (IActor)entity;
                        await Attack(actor, animationSpeedFactor);
                        break;
                    case EntityType.Terrain: continue;
                    default: throw new Exception("未定義のEntityType");
                }
                isActionComplete = true;
            }
        }

        private async UniTask Attack(IActor target, float animationSpeedFactor)
        {
            body.Attack(target.Body);
            await UniTask.Delay(100);
        }

        private async UniTask Move(ActorDirection direction, float animationSpeedFactor)
        {
            var endPoint = gridMovement.StepTo(direction);
            await visual.StepAnimation(endPoint, animationSpeedFactor);
        }
    }
}