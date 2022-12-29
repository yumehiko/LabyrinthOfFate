using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// 経路を探索し、Playerに向かって進む。
    /// </summary>
    public class PathFindMelee : ActorBrainBase
    {
        public override IActorModel Model => model;
        public override IActorView View => view;

        private readonly Level level;
        private readonly IActorModel model;
        private readonly IActorView view;

        public PathFindMelee(Level level, IActorModel model, IActorView view)
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
            var start = model.Position;
            var end = level.Actors.GetPlayerPosition();
            var path = level.Dungeon.FindPath(start, end);

            //経路がない場合は、その場で待機。
            if (path.Length <= 1)
            {
                await view.WaitAnimation(start, animationSpeedFactor, token);
                return;
            }

            //経路番号1がPlayerの位置なら、隣接しているので攻撃する。
            if (path[1] == level.Actors.GetPlayerPosition())
            {
                model.Attack(level.Actors.Player);
                await view.AttackAnimation(path[1], animationSpeedFactor, token);
                return;
            }

            //移動先にPlayer以外のActorがいる場合、単に停止する。
            if (level.Actors.GetEnemyAt(path[1]) != null)
            {
                await view.WaitAnimation(start, animationSpeedFactor, token);
                return;
            }

            //それ以外の場合、経路1へ移動する。
            model.StepTo(path[1]);
            await view.StepAnimation(path[1], animationSpeedFactor, token);
        }
    }
}