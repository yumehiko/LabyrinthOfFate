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

        //TODO: 微妙な変数なので整頓したい。
        private bool canAction = true;

        private void Start()
        {
            //移動入力
            ReactiveInput.OnMove
                .Where(_ => canAction)
                .Where(value => value != ActorDirection.None)
                .Subscribe(_ => InputMove().Forget())
                .AddTo(this);
        }

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public async UniTask DoTurnAction(float timeFactor)
        {
            var direction = await ReactiveInput.OnMove;
            //TODO
            //その方向に行動できるかを確認する。
            //その方向に行動する。
            //遠いTODO
            //アイテムを使用したりする。
        }

        private async UniTask InputMove()
        {
            //最初に呼び出された瞬間から、入力猶予（0.2秒とか）待ち、その時入力されていたカーソル方向へ移動にトライする。
            canAction = false;
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            var direction = ReactiveInput.OnMove.Value;
            //ここで実際に移動可能かをチェックする。不可能なら無視する。
            IEntity entity = gridMovement.CheckEntityTo(direction);
            var entityType = entity == null ? EntityType.None : entity.EntityType;
            Debug.Log(entityType);
            if (entityType != EntityType.None)
            {
                canAction = true;
                return;
            }
            //NPCとかは移動可能な方向に進むはずだが、プレイヤーの場合は壁に進もうとする場合もあるので、チェックするクラスも必要になる。
            //（ランダムムーブNPCとかいるかもしれんが）
            var endPoint = gridMovement.StepTo(direction);
            await visual.StepAnimation(endPoint, 0.25f);
            ReactiveInput.ClearDirection();
            canAction = true;
        }
    }
}