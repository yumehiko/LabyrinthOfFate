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
        public ActorType ActorType => ActorType.Player;
        public IActor Model => model;
        public IActorView View => view;

        private readonly Adventure adventure;
        private readonly IActor model;
        private readonly IActorView view;

        private readonly AsyncReactiveProperty<ActorDirection> inputDirection = new AsyncReactiveProperty<ActorDirection>(ActorDirection.None);
        private readonly Subject<Unit> inputWait = new Subject<Unit>();
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private bool canControl = false;

        public Player(Adventure adventure, IActor model, IActorView view)
        {
            this.adventure = adventure;
            this.model = model;
            this.view = view;

            ReactiveInput.OnMove
                .Where(_ => canControl)
                .Subscribe(value => inputDirection.Value = value)
                .AddTo(disposables);

            ReactiveInput.OnWait
                .Where(_ => canControl)
                .Where(isTrue => isTrue)
                .Subscribe(_ => inputWait.OnNext(Unit.Default))
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
            canControl = true;
            //移動入力・UI入力を待つ。
            var inputs = new List<UniTask>
            {
                InputDirection(animationSpeedFactor, token),
                InputWait(animationSpeedFactor, token),
            };

            //いずれかのターン消費行動が確定したら、行動終了。
            await UniTask.WhenAny(inputs);
        }

        public void WarpTo(Vector2Int position)
        {
            model.WarpTo(position);
            view.WarpTo(position);
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
                var targetPoint = model.GetPositionWithDirection(direction);

                //指定地点にEnemyがいないかをチェックする。
                IActor enemy = adventure.CurrentLevel.Actors.GetEnemyAt(targetPoint);
                if (enemy != null) //Enemyがいるなら、それを攻撃する。
                {
                    canControl = false;
                    model.Attack(enemy);
                    await view.AttackAnimation(targetPoint, animationSpeedFactor, token);
                    break;
                }

                //指定地点の地形をチェックする。
                var tileType = adventure.CurrentLevel.Dungeon.GetTileType(targetPoint);
                if (tileType == TileType.Empty)
                {
                    canControl = false;
                    model.StepTo(targetPoint);
                    await view.StepAnimation(targetPoint, animationSpeedFactor, token);
                    break;
                }
            }
        }

        private async UniTask InputWait(float animationSpeedFactor, CancellationToken token)
        {
            await inputWait.ToUniTask(true, token);
            canControl = false;
            await view.WaitAnimation(model.Position, animationSpeedFactor, token);
        }
    }
}