﻿using System.Collections;
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
        public IObservable<Unit> OnPlayerIsDead => onPlayerIsDead;
        public IObservable<Unit> OnDefeatAllEnemies => onDefeatAllEnemies;
        public int TurnCount { get; private set; }

        private readonly Subject<Unit> onPlayerIsDead = new Subject<Unit>();
        private readonly Subject<Unit> onDefeatAllEnemies = new Subject<Unit>();
        private readonly Subject<Unit> onPlayerActEnd = new Subject<Unit>();
        private readonly List<UniTask> enemyTasks = new List<UniTask>();
        private CancellationTokenSource logicCTS;
        private CancellationTokenSource animationCTS;

        /// <summary>
        /// ターンループを開始する。
        /// </summary>
        public async UniTaskVoid StartTurnLoop(Actors actors, CancellationToken levelCT)
        {
            while (!levelCT.IsCancellationRequested)
            {
                var someoneHadEnergy = await DoTurn(actors, levelCT);
                if (!someoneHadEnergy) //全員のEnergyが尽きたなら、全員Energyをリセットして次のターンへ
                {
                    RefleshAllEnergy(actors);
                    TurnCount++;
                    Debug.Log($"Turn: {TurnCount}");
                }
            }
        }

        public void Dispose()
        {
            logicCTS?.Cancel();
            logicCTS?.Dispose();
            animationCTS?.Cancel();
            animationCTS?.Dispose();
        }

        /// <summary>
        /// アニメーションを即時・強制的に完了する。
        /// </summary>
        public void ForceCompleteAnimation()
        {
            animationCTS?.Cancel();
            animationCTS?.Dispose();
        }

        private async UniTask<bool> DoTurn(Actors actors, CancellationToken levelCT)
        {
            try
            {
                logicCTS = new CancellationTokenSource();
                animationCTS = new CancellationTokenSource();
                var playerActRequest = new ActRequest(1.0f, logicCTS.Token, animationCTS.Token);
                var enemyActRequest = new ActRequest(0.5f, logicCTS.Token, animationCTS.Token);
                bool someoneHadEnergy = false;
                someoneHadEnergy = await DoPlayerTurn(actors.Player, playerActRequest, levelCT);
                someoneHadEnergy = await DoEnemiesTurn(actors.Enemies, enemyActRequest, levelCT) || someoneHadEnergy;
                return someoneHadEnergy;
            }
            finally
            {
                enemyTasks.Clear();
                logicCTS.Cancel();
                logicCTS.Dispose();
                animationCTS?.Cancel();
                animationCTS?.Dispose();
                logicCTS = null;
                animationCTS = null;

                if (actors.Player.Model.IsDied.Value)
                {
                    onPlayerIsDead.OnNext(Unit.Default);
                }

                if (actors.Enemies.Count == 0)
                {
                    onDefeatAllEnemies.OnNext(Unit.Default);
                }
            }
        }

        /// <summary>
        /// プレイヤーのターン
        /// </summary>
        /// <param name="player"></param>
        /// <param name="levelCT"></param>
        /// <returns></returns>
        private async UniTask<bool> DoPlayerTurn(IActorBrain player, ActRequest request, CancellationToken levelCT)
        {
            bool hadEnergy = false;
            if (player.HasEnergy)
            {
                await player.DoTurnAction(request);
                levelCT.ThrowIfCancellationRequested();
                onPlayerActEnd.OnNext(Unit.Default);
                hadEnergy = true;
            }
            return hadEnergy;
        }

        /// <summary>
        /// 敵のターン
        /// </summary>
        /// <param name="enemies"></param>
        /// <param name="levelCT"></param>
        /// <returns></returns>
        private async UniTask<bool> DoEnemiesTurn(IReadOnlyList<IActorBrain> enemies, ActRequest request, CancellationToken levelCT)
        {
            //TODO:アニメーション用のキャンセルトークンを作っておくと、アニメーション動作だけほっといてプレイヤーターンへ移れるかもしれん。
            //あと、挙動によってはアニメーションの終わりを待つべき重要なアクションはあるかもしれん。
            //Brainからは指令だけもらって、ActorPresenterを分割して、Presenterにmodel指令とview指令を別個に送り、view指令だけwhenAllする？
            //指令によっては待つとかできる。
            //TODO:途中で敵が自爆したりするとInvaildするかも？　新たにリストを作ってみたいな処理がいるか？

            bool hadEnergy = false;
            foreach (IActorBrain enemy in enemies)
            {
                if (enemy.HasEnergy)
                {
                    enemyTasks.Add(enemy.DoTurnAction(request));
                    await UniTask.DelayFrame(2, cancellationToken: request.AnimationCT);
                    levelCT.ThrowIfCancellationRequested();
                    hadEnergy = true;
                }
            }
            await UniTask.WhenAll(enemyTasks);
            return hadEnergy;
        }

        private void RefleshAllEnergy(Actors actors)
        {
            actors.Player.RefleshEnergy();
            foreach (var enemy in actors.Enemies)
            {
                enemy.RefleshEnergy();
            }
        }
    }
}