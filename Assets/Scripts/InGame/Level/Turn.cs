using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;


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
                await DoTurn();
            }
        }

        public void EndTurnLoop()
        {
            isTurnLooping = true;
        }

        private async UniTask DoTurn()
        {
            foreach(IActor player in players)
            {
                await player.DoTurnAction(1.0f);
            }

            foreach (IActor enemy in enemies)
            {
                await enemy.DoTurnAction(1.0f);
            }
        }
    }
}