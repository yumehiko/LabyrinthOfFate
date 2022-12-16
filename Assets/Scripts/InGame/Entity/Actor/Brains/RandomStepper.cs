﻿using System.Collections;
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
	public class RandomStepper : IActorBrain
    {
        private readonly Floor floor;
        private readonly Entities entities;
        private readonly ActorBody body;
        private readonly IActorView view;

        public RandomStepper(Floor floor, Entities entities, ActorBody body, IActorView view)
        {
            this.floor = floor;
            this.entities = entities;
            this.body = body;
            this.view = view;
        }

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public async UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token)
        {
            await RandomStep(animationSpeedFactor, token);
        }

        private async UniTask RandomStep(float animationSpeedFactor, CancellationToken token)
        {
            //このままだと攻撃はできない。
            while (true)
            {
                ActorDirection direction = GetRandomDirection();
                var point = body.GetPositionWithDirection(direction);

                //方向指定がない場合は、その場で待機。
                if (direction == ActorDirection.None)
                {
                    await view.WaitAnimation(animationSpeedFactor, token);
                    return;
                }

                //Playerがいるなら攻撃する。
                var player = entities.GetPlayerAt(point);
                if (player != null)
                {
                    body.Attack(player);
                    await view.AttackAnimation(animationSpeedFactor, token);
                    return;
                }

                //地形が空なら移動する。
                if (floor.GetTerrainType(point) == FloorType.Empty)
                {
                    body.StepTo(point);
                    await view.StepAnimation(animationSpeedFactor, token);
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