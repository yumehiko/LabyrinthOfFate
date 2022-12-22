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

        private readonly Subject<Unit> onPlayerActEnd = new Subject<Unit>();
        private readonly List<UniTask> enemyTasks = new List<UniTask>();
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
                await UniTask.WhenAll(enemyTasks);
                turnTokenSource?.Cancel();
                turnTokenSource?.Dispose();
            }
        }

        private async UniTask TurnLoop(IActorBrain player, IReadOnlyList<IActorBrain> enemies, CancellationToken levelCancelToken)
        {
            while (true)
            {
                levelCancelToken.ThrowIfCancellationRequested();
                turnTokenSource = new CancellationTokenSource();
                //プレイヤーターン
                await player.DoTurnAction(1.0f, turnTokenSource.Token);
                levelCancelToken.ThrowIfCancellationRequested();
                onPlayerActEnd.OnNext(Unit.Default);

                //エネミーターン
                //TODO:アニメーション用のキャンセルトークンを作っておくと、アニメーション動作だけほっといてプレイヤーターンへ移れるかもしれん。
                //あと、挙動によってはアニメーションの終わりを待つべき重要なアクションはあるかもしれん。
                //Brainからは指令だけもらって、ActorPresenterを分割して、Presenterにmodel指令とview指令を別個に送り、view指令だけwhenAllする？
                //指令によっては待つとかできる。
                //TODO:途中で敵が自爆したりするとInvaildするかも？
                foreach (IActorBrain enemy in enemies)
                {
                    enemyTasks.Add(enemy.DoTurnAction(0.5f, turnTokenSource.Token));
                    await UniTask.DelayFrame(2, cancellationToken: turnTokenSource.Token);
                    levelCancelToken.ThrowIfCancellationRequested();
                }
                await UniTask.WhenAll(enemyTasks);
                enemyTasks.Clear();

                turnTokenSource.Cancel();
                turnTokenSource.Dispose();
            }
        }
    }
}