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

        /// <summary>
        /// ターンシステムを起動する。
        /// </summary>
        public void Startup(IReadOnlyList<IActor> players, IReadOnlyList<IActor> enemies)
        {
            this.players = players;
            this.enemies = enemies;
        }

        private void DoTurn()
        {

        }
    }
}