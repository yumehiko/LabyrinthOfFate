using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using yumehiko.Input;
using VContainer;
using VContainer.Unity;

namespace yumehiko.LOF
{
    public class Player : MonoBehaviour, IActor
    {
        [SerializeField] private GridMovement gridMovement;
        [SerializeField] private ActorVisual visual;

        public EntityType EntityType => EntityType.Actor;
        public Affiliation Affiliation => Affiliation.Player;

        private PlayerControlMode controlMode = PlayerControlMode.Move;

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public async UniTask DoTurnAction(float timeFactor)
        {
            //移動入力・UI入力を待つ。
            var inputs = new List<UniTask>
            {
                InputMove(),
            };
            await UniTask.WhenAny(inputs);
        }

        private async UniTask InputMove()
        {
            bool isMoveConfirm = false;

            while (!isMoveConfirm)
            {
                ReactiveInput.ClearDirection();
                await UniTask.WaitUntil(() => controlMode == PlayerControlMode.Move);
                //最初に呼び出された瞬間から、入力猶予（0.1秒とか）待ち、その時入力されていたカーソル方向へ移動にトライする。
                var direction = await ReactiveInput.OnMove;
                //ここで実際に移動可能かをチェックする。不可能なら無視する。
                IEntity entity = gridMovement.CheckEntityTo(direction);
                var entityType = entity == null ? EntityType.None : entity.EntityType;
                if (entityType != EntityType.None)
                {
                    continue;
                }
                var endPoint = gridMovement.StepTo(direction);
                await visual.StepAnimation(endPoint, 0.25f);
                ReactiveInput.ClearDirection();
                isMoveConfirm = true;
            }
        }
    }
}