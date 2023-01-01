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
        public override IActorModel Model { get; }
        public override IActorView View { get; }
        private readonly Level level;

        public PathFindMelee(Level level, IActorModel model, IActorView view)
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
                Debug.Log($"{Model.Inventory.Count}");
                //インベントリにカードがあるなら、それをInvokeする。
                if(Model.Inventory.Count > 0)
                {
                    Model.Inventory.InvokeCard(0, Model);
                    return;
                }
                await StepOrAttack(animationSpeedFactor, token);
            }
            finally
            {
                Model.Status.UseEnergy();
            }
        }

        /// <summary>
        /// 移動または攻撃
        /// </summary>
        /// <param name="animationSpeedFactor"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask StepOrAttack(float animationSpeedFactor, CancellationToken token)
        {
            var start = Model.Position;
            var end = level.Actors.GetPlayerPosition();
            var path = level.Dungeon.FindPath(start, end);

            //経路がない場合は、その場で待機。
            if (path.Length <= 1)
            {
                await View.WaitAnimation(start, animationSpeedFactor, token);
                return;
            }

            //経路番号1がPlayerの位置なら、隣接しているので攻撃する。
            if (path[1] == level.Actors.GetPlayerPosition())
            {
                Model.Attack(level.Actors.Player.Model);
                await View.AttackAnimation(path[1], animationSpeedFactor, token);
                return;
            }

            //移動先にPlayer以外のActorがいる場合、単に停止する。
            if (level.Actors.GetEnemyAt(path[1]) != null)
            {
                await View.WaitAnimation(start, animationSpeedFactor, token);
                return;
            }

            //それ以外の場合、経路1へ移動する。
            Model.StepTo(path[1]);
            await View.StepAnimation(path[1], animationSpeedFactor, token);
        }
    }
}