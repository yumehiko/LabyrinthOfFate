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
	public class PlayerInput : IStartable, IDisposable
	{
        //TODO: 状態enumにする。
        private bool canAction = true;
        private CompositeDisposable disposables = new CompositeDisposable();

        private readonly GridMovement gridMovement;

        [Inject]
        public PlayerInput(GridMovement gridMovement)
        {
            this.gridMovement = gridMovement;
        }

        public void Start()
        {
            //移動入力
            ReactiveInput.OnMove
                .Where(_ => canAction)
                .Where(value => value != ActorDirection.None)
                .Subscribe(_ => Move().Forget())
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        private async UniTask Move()
        {
            //最初に呼び出された瞬間から、入力猶予（0.2秒とか）待ち、その時入力されていたカーソル方向へ移動にトライする。
            canAction = false;
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            var direction = ReactiveInput.OnMove.Value;
            //TODO: ここで実際に移動可能かをチェックする。
            //NPCとかは移動可能な方向に進むはずだが、プレイヤーの場合は壁に進もうとする場合もあるので、チェックするクラスも必要になる。
            //（ランダムムーブNPCとかいるかもしれんが）
            gridMovement.StepTo(direction);
            ReactiveInput.ClearDirection();
            canAction = true;
        }
    }
}