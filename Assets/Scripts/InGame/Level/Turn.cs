using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.LOF
{

    /// <summary>
    /// このゲームにおけるターンを表す。
    /// </summary>
    public class Turn : MonoBehaviour
    {
        private IReadOnlyList<IActor> players;
        private IReadOnlyList<IActor> enemies;
        private bool isTurnLooping = false;

        private CancellationTokenSource actionCancelTokenSource = new CancellationTokenSource();

        private void OnDestroy()
        {
            actionCancelTokenSource.Cancel();
            actionCancelTokenSource.Dispose();
        }

        /// <summary>
        /// ターンシステムを起動する。
        /// </summary>
        public async UniTaskVoid Startup(IReadOnlyList<IActor> players, IReadOnlyList<IActor> enemies)
        {
            this.players = players;
            this.enemies = enemies;

            //ターンループ開始。
            isTurnLooping = true;
            while(isTurnLooping)
            {
                await DoTurn(actionCancelTokenSource.Token);
                actionCancelTokenSource.Cancel();
                actionCancelTokenSource.Dispose();
                actionCancelTokenSource = new CancellationTokenSource();
            }
        }

        public void EndTurnLoop()
        {
            isTurnLooping = true;
        }

        private async UniTask DoTurn(CancellationToken token)
        {
            foreach(IActor player in players)
            {
                await player.DoTurnAction(1.0f, token);
            }

            foreach (IActor enemy in enemies)
            {
                await enemy.DoTurnAction(0.5f, token);
            }
        }
    }
}