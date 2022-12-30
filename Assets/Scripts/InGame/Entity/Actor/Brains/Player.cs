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
        public override IActorModel Model { get; }
        public override IActorView View { get; }
        public Inventory Inventory { get; }

        private Adventure adventure;

        private readonly AsyncReactiveProperty<ActorDirection> inputDirection = new AsyncReactiveProperty<ActorDirection>(ActorDirection.None);
        private readonly Subject<Unit> inputWait = new Subject<Unit>();
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private bool canTurnControl = false;

        [Inject]
        public Player(Inventory inventory, PlayerProfile profile, Actors actors)
        {
            Inventory = inventory;
            Model = new ActorModel(profile, Vector2Int.zero);
            View = UnityEngine.Object.Instantiate(profile.View);
            actors.AddPlayer(this, Model, View);
            inventory.InitializeEquips(Model.Status.EquipSlot);

            _ = ReactiveInput.OnMove
                 .Where(_ => canTurnControl)
                 .Subscribe(value => inputDirection.Value = value)
                 .AddTo(disposables);

            _ = ReactiveInput.OnWait
                .Where(_ => canTurnControl)
                .Where(isTrue => isTrue)
                .Subscribe(_ => inputWait.OnNext(Unit.Default))
                .AddTo(disposables);

            _ = ReactiveInput.OnInventory
                .Where(_ => canTurnControl)
                .Where(isTrue => isTrue)
                .Subscribe(_ => inventory.SwitchOpen())
                .AddTo(disposables);

            _ = inventory.OnInvokeEffect
                .Subscribe(item => item.InvokeEffect.Invoke(this, item))
                .AddTo(disposables);

            _ = Model.IsDied
                .Where(isTrue => isTrue)
                .First()
                .Subscribe(_ => OnDied())
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
            canTurnControl = true;
            //移動入力・UI入力を待つ。
            var inputs = new List<UniTask>
            {
                InputDirection(animationSpeedFactor, token),
                InputWait(animationSpeedFactor, token),
                Inventory.OnCommand.ToUniTask(true, token),
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
                var targetPoint = Model.GetPositionWithDirection(direction);

                //指定地点にEnemyがいないかをチェックする。
                IActorModel enemy = adventure.CurrentLevel.Actors.GetEnemyAt(targetPoint);
                if (enemy != null) //Enemyがいるなら、それを攻撃する。
                {
                    TurnInputConfirm();
                    Model.Attack(enemy);
                    await View.AttackAnimation(targetPoint, animationSpeedFactor, token);
                    break;
                }

                //指定地点の地形をチェックする。
                var tileType = adventure.CurrentLevel.Dungeon.GetTileType(targetPoint);
                if (tileType == TileType.Empty)
                {
                    TurnInputConfirm();
                    Model.StepTo(targetPoint);
                    await View.StepAnimation(targetPoint, animationSpeedFactor, token);
                    break;
                }
            }
        }

        private async UniTask InputWait(float animationSpeedFactor, CancellationToken token)
        {
            await inputWait.ToUniTask(true, token);
            TurnInputConfirm();
            await View.WaitAnimation(Model.Position, animationSpeedFactor, token);
        }

        /// <summary>
        /// ターン入力が確定したので、他の行動を入力できないようにする。
        /// </summary>
        private void TurnInputConfirm()
        {
            canTurnControl = false;
            Inventory.Close();
        }

        /// <summary>
        /// 死亡時処理。
        /// </summary>
        private void OnDied()
        {
            canTurnControl = false;
            Inventory.Close();
        }
    }
}