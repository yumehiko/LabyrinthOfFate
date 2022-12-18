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
    public class Turn : IDisposable
    {
        public IObservable<Unit> OnPlayerActEnd => onPlayerActEnd;

        private Subject<Unit> onPlayerActEnd = new Subject<Unit>();
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// ターンループを開始する。
        /// </summary>
        public async UniTaskVoid StartTurnLoop(IActorBrain player, IReadOnlyList<IActorBrain> enemies)
        {
            var token = cancellationTokenSource.Token;

            try
            {
                await TurnLoop(player, enemies, token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("ターンループ終了");
                Dispose();
            }
        }

        private async UniTask TurnLoop(IActorBrain player, IReadOnlyList<IActorBrain> enemies, CancellationToken token)
        {
            while (true)
            {
                //プレイヤーターン
                token.ThrowIfCancellationRequested();
                await player.DoTurnAction(1.0f, token);
                onPlayerActEnd.OnNext(Unit.Default);

                //エネミーターン
                foreach (IActorBrain enemy in enemies)
                {
                    token.ThrowIfCancellationRequested();
                    enemy.DoTurnAction(0.5f, token).Forget();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: token);
                }
            }
        }

        /// <summary>
        /// ターンループを終了する。
        /// </summary>
        public void EndTurnLoop()
        {
            cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
    }
}