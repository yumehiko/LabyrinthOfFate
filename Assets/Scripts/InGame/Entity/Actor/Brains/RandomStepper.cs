using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// 毎ターンランダムに移動する。
    /// </summary>
	public class RandomStepper : ActorBrainBase
    {
        public override IActorModel Model { get; }
        public override IActorView View { get; }

        private readonly Level level;

        public RandomStepper(Level level, IActorModel model, IActorView view)
        {
            this.level = level;
            this.Model = model;
            this.View = view;
        }

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public override async UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token)
        {
            try
            {
                await RandomStep(animationSpeedFactor, token);
            }
            finally
            {
                Model.Status.UseEnergy();
            }
        }

        private async UniTask RandomStep(float animationSpeedFactor, CancellationToken token)
        {
            while (true)
            {
                ActorDirection direction = GetRandomDirection();
                var point = Model.GetPositionWithDirection(direction);

                //方向指定がない場合は、その場で待機。
                if (direction == ActorDirection.None)
                {
                    await View.WaitAnimation(point, animationSpeedFactor, token);
                    return;
                }

                //指定地点にActorがいるか確認する。
                var actor = level.Actors.GetActorAt(point);

                //それがプレイヤーなら攻撃する。
                if (actor != null && actor.ActorType == ActorType.Player)
                {
                    Model.Attack(actor);
                    await View.AttackAnimation(point, animationSpeedFactor, token);
                    return;
                }

                //それがプレイヤー以外なら、行動入力へ戻る。
                if (actor != null)
                {
                    continue;
                }

                //Actorがいない上に、地形が空なら移動する。
                if (level.Dungeon.GetTileType(point) == TileType.Empty)
                {
                    Model.StepTo(point);
                    await View.StepAnimation(point, animationSpeedFactor, token);
                    return;
                }
            }
        }

        private ActorDirection GetRandomDirection()
        {
            var enumArray = System.Enum.GetValues(typeof(ActorDirection));
            var randomID = Random.Range(0, enumArray.Length);
            return (ActorDirection)enumArray.GetValue(randomID);
        }
    }
}