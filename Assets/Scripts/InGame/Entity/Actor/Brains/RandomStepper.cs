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
        public override IActorModel Model => model;
        public override IActorView View => view;

        private readonly Level level;
        private readonly IActorModel model;
        private readonly IActorView view;

        public RandomStepper(Level level, IActorModel model, IActorView view)
        {
            this.level = level;
            this.model = model;
            this.view = view;
        }

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public override async UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token)
        {
            await RandomStep(animationSpeedFactor, token);
        }

        private async UniTask RandomStep(float animationSpeedFactor, CancellationToken token)
        {
            while (true)
            {
                ActorDirection direction = GetRandomDirection();
                var point = model.GetPositionWithDirection(direction);

                //方向指定がない場合は、その場で待機。
                if (direction == ActorDirection.None)
                {
                    await view.WaitAnimation(point, animationSpeedFactor, token);
                    return;
                }

                //指定地点にActorがいるか確認する。
                var actor = level.Actors.GetActorAt(point);

                //それがプレイヤーなら攻撃する。
                if (actor != null && actor.ActorType == ActorType.Player)
                {
                    model.Attack(actor);
                    await view.AttackAnimation(point, animationSpeedFactor, token);
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
                    model.StepTo(point);
                    await view.StepAnimation(point, animationSpeedFactor, token);
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