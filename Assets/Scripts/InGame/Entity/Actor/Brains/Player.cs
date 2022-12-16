using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using yumehiko.Input;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// プレイヤーキャラクターのゲーム中の実体。その操作処理。
    /// </summary>
    public class Player : IActorBrain, IDisposable
    {
        public EntityType EntityType => EntityType.Actor;
        public ActorType ActorType => ActorType.Player;
        public ActorBody Body => body;

        private readonly Floor floor;
        private readonly Entities entities;
        private readonly ActorBody body;
        private readonly IActorView view;

        private readonly AsyncReactiveProperty<ActorDirection> inputDirection = new AsyncReactiveProperty<ActorDirection>(ActorDirection.None);
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public Player(Floor floor, Entities entities, ActorBody body, IActorView view)
        {
            this.floor = floor;
            this.entities = entities;
            this.body = body;
            this.view = view;

            ReactiveInput.OnMove
                .Subscribe(value => inputDirection.Value = value)
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
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

        /// <summary>
        /// 方向入力による行動。
        /// </summary>
        /// <param name="animationSpeedFactor"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask InputDirection(float animationSpeedFactor, CancellationToken token)
        {
            ReactiveInput.ClearDirection();

            while (true)
            {
                var direction = await inputDirection.WaitAsync(token);
                var point = body.DirectionToVector(direction);

                //指定地点にEnemyがいないかをチェックする。
                IActor enemy = entities.GetEnemyAt(point);
                if (enemy != null) //Enemyがいるなら、それを攻撃する。
                {
                    body.Attack(enemy);
                    await view.AttackAnimation(animationSpeedFactor, token);
                    break;
                }

                //指定地点の地形をチェックする。
                var floorType = floor.GetTerrainType(point);
                if (floorType == FloorType.Empty)
                {
                    body.StepTo(point);
                    await view.StepAnimation(animationSpeedFactor, token);
                    break;
                }
            }
        }
    }
}