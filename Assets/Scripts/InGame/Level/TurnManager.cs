using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;


namespace yumehiko.LOF
{

    /// <summary>
    /// レベルのエンティティのターンを管理する。
    /// </summary>
    public class TurnManager : MonoBehaviour
    {

        /// <summary>
        /// ターン開始。
        /// </summary>
        public System.IObservable<Unit> OnStart => onStart;

        /// <summary>
        /// 経路探索タイミング。
        /// </summary>
        public System.IObservable<Unit> OnPathFind => onPathFind;

        /// <summary>
        /// 邪眼の影響更新タイミング。
        /// </summary>
        public System.IObservable<Unit> OnEvilSightReflesh => onEvilSightReflesh;

        /// <summary>
        /// 捕獲確認タイミング。
        /// </summary>
        public System.IObservable<Unit> OnCapture => onCapture;

        /// <summary>
        /// 石化確認タイミング。
        /// </summary>
        public System.IObservable<Unit> OnPetrify => onPetrify;

        /// <summary>
        /// 先行入力実行タイミング。
        /// </summary>
        public System.IObservable<Unit> OnSolveInputBuffer => onSolveInputBuffer;

        /// <summary>
        /// ターン実行中のTween。
        /// </summary>
        public Tween TurnTween { get; private set; } = default;

        /// <summary>
        /// ターン動作が実行中か。
        /// </summary>
        public bool IsActing { get; private set; } = false;

        private Subject<Unit> onStart = new Subject<Unit>();
        private Subject<Unit> onPathFind = new Subject<Unit>();
        private Subject<Unit> onEvilSightReflesh = new Subject<Unit>();
        private Subject<Unit> onCapture = new Subject<Unit>();
        private Subject<Unit> onPetrify = new Subject<Unit>();
        private Subject<Unit> onSolveInputBuffer = new Subject<Unit>();



        /// <summary>
        /// ターンを進める。
        /// </summary>
        public async UniTask TurnStart(float duration)
        {
            onStart.OnNext(Unit.Default);
            onPathFind.OnNext(Unit.Default);

            //ターン動作（アニメーションや移動）の実行。
            IsActing = true;
            //MEMO: なんでawait?
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration));
            TurnEnd();
        }

        /// <summary>
        /// ターンを終了する。
        /// </summary>
        private void TurnEnd()
        {
            onEvilSightReflesh.OnNext(Unit.Default);
            onCapture.OnNext(Unit.Default);
            onPetrify.OnNext(Unit.Default);

            //ターン動作実行完了。
            IsActing = false;

            //先行入力の確認。
            onSolveInputBuffer.OnNext(Unit.Default);
        }
    }
}