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
                token.ThrowIfCancellationRequested();
                await player.DoTurnAction(1.0f, token);
                foreach (IActorBrain enemy in enemies)
                {
                    token.ThrowIfCancellationRequested();
                    await enemy.DoTurnAction(0.5f, token);
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