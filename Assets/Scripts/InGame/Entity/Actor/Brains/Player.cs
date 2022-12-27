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
using VContainer;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// プレイヤーキャラクターのゲーム中の実体。その操作処理。
    /// </summary>
    public class Player : ActorBrainBase, IDisposable
    {
        public ActorType ActorType => ActorType.Player;
        public override IActor Model => model;
        public override IActorView View => view;

        public Inventory Inventory { get; }

        private Adventure adventure;
        private readonly IActor model;
        private readonly IActorView view;

        private readonly AsyncReactiveProperty<ActorDirection> inputDirection = new AsyncReactiveProperty<ActorDirection>(ActorDirection.None);
        private readonly Subject<Unit> inputWait = new Subject<Unit>();
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private bool canControl = false;

        [Inject]
        public Player(Inventory inventory, PlayerProfile profile, ActorPresenters actorPresenters)
        {
            this.Inventory = inventory;
            model = new Actor(profile, Vector2Int.zero);
            view = UnityEngine.Object.Instantiate(profile.View);
            actorPresenters.AddPlayer(this, model, view);

            _ = ReactiveInput.OnMove
                 .Where(_ => canControl)
                 .Subscribe(value => inputDirection.Value = value)
                 .AddTo(disposables);

            _ = ReactiveInput.OnWait
                .Where(_ => canControl)
                .Where(isTrue => isTrue)
                .Subscribe(_ => inputWait.OnNext(Unit.Default))
                .AddTo(disposables);
        }

        public void Initialize(Adventure adventure)
        {
            this.adventure = adventure;
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public override async UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token)
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