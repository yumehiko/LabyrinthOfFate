using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Toolkit;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace yumehiko.LOF.Presenter
{

    /// <summary>
    /// このゲームにおけるターンを表す。
    /// </summary>
    public class Turn
    {
        public IObservable<Unit> OnPlayerActEnd => onPlayerActEnd;

        private Subject<Unit> onPlayerActEnd = new Subject<Unit>();
        private CancellationTokenSource turnTokenSource;

        /// <summary>
        /// ターンループを開始する。
        /// </summary>
        public async UniTaskVoid StartTurnLoop(IActorBrain player, IReadOnlyList<IActorBrain> enemies, CancellationToken levelCancellToken)
        {
            try
            {
                await TurnLoop(player, enemies, levelCancellToken);
            }
            catch (OperationCanceledException)
            {
                turnTokenSource?.Cancel();
                turnTokenSource?.Dispose();
                Debug.Log("ターンループ終了");
            }
        }

        private async UniTask TurnLoop(IActorBrain player, IReadOnlyList<IActorBrain> enemies, CancellationToken turnCancelToken)
        {
            while (true)
            {
                turnTokenSource = new CancellationTokenSource();
                //プレイヤーターン
                turnCancelToken.ThrowIfCancellationRequested();
                await player.DoTurnAction(1.0f, turnTokenSource.Token);
                onPlayerActEnd.OnNext(Unit.Default);

                List<UniTask> enemyTasks = new List<UniTask>();
                //エネミーターン
                //TODO:アニメーション用のキャンセルトークンを作っておくと、アニメーション動作だけほっといてプレイヤーターンへ移れるかもしれん。
                //あと、挙動によってはアニメーションの終わりを待つべき重要なアクションはあるかもしれん。
                foreach (IActorBrain enemy in enemies)
                {
                    turnCancelToken.ThrowIfCancellationRequested();
                    enemyTasks.Add(enemy.DoTurnAction(0.5f, turnTokenSource.Token));
                    await UniTask.DelayFrame(2, cancellationToken: turnTokenSource.Token);
                }
                await UniTask.WhenAll(enemyTasks);

                turnTokenSource.Cancel();
                turnTokenSource.Dispose();
            }
        }
    }
}