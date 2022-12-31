using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using yumehiko.LOF;

namespace yumehiko.Input
{
    /// <summary>
    /// プレイヤーの入力（コントローラーやキーボード）を監視・管理する。
    /// </summary>
    public static class ReactiveInput
    {
        private static UserInputs inputActions;

        /// <summary>
        /// 視点操作の感度。
        /// </summary>
        public static float MouseLookSensitivity { get; set; } = 1.0f;

        /// <summary>
        /// 視点操作において、Y軸を反転するか。
        /// </summary>
        public static bool InvertYAxis { get; set; } = false;

        /// <summary>
        /// 方向操作入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<ActorDirection> OnMove => onMove;

        /// <summary>
        /// 待機入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnWait => onWait;

        /// <summary>
        /// ポインター（マウス）操作の入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<Vector2> OnPointer => onPointer;

        /// <summary>
        /// 右スティック操作の入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<Vector2> OnRightStick => onRightStick;

        /// <summary>
        /// マルボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnMaru => onMaru;

        /// <summary>
        /// ペケボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnPeke => onPeke;

        /// <summary>
        /// ポーズボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnPause => onPause;

        /// <summary>
        /// リスタートボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnRestart => onRestart;

        /// <summary>
        /// インベントリボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnInventory => onInventory;

        /// <summary>
        /// デバッグキー入力
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnDebug => onDebug;


        private static readonly Vector2ReactiveProperty onPointer = new Vector2ReactiveProperty();
        private static readonly Vector2ReactiveProperty onRightStick = new Vector2ReactiveProperty();
        private static readonly BoolReactiveProperty onMaru = new BoolReactiveProperty();
        private static readonly BoolReactiveProperty onPeke = new BoolReactiveProperty();
        private static readonly BoolReactiveProperty onPause = new BoolReactiveProperty();
        private static readonly BoolReactiveProperty onRestart = new BoolReactiveProperty();
        private static readonly BoolReactiveProperty onInventory = new BoolReactiveProperty();
        private static readonly ActorDirectionReactiveProperty onMove = new ActorDirectionReactiveProperty();
        private static readonly BoolReactiveProperty onWait = new BoolReactiveProperty();
        private static readonly BoolReactiveProperty onDebug = new BoolReactiveProperty();



        static ReactiveInput()
        {
            SubscribeInputs();
        }


        private static void SubscribeInputs()
        {
            //inputActionを生成
            inputActions = new UserInputs();
            inputActions.Enable();

            //各操作入力のON/OFFを購読。
            inputActions.Player.Maru.started += context => onMaru.Value = true;
            inputActions.Player.Maru.canceled += context => onMaru.Value = false;

            inputActions.Player.Peke.started += context => onPeke.Value = true;
            inputActions.Player.Peke.canceled += context => onPeke.Value = false;

            inputActions.Player.Pointer.performed += context => onPointer.Value = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

            inputActions.Player.RightStick.performed += context => onRightStick.Value = context.ReadValue<Vector2>();
            inputActions.Player.RightStick.canceled += context => onRightStick.Value = context.ReadValue<Vector2>();

            inputActions.Player.Pause.started += context => onPause.Value = true;
            inputActions.Player.Pause.canceled += context => onPause.Value = false;

            inputActions.Player.Restart.started += context => onRestart.Value = true;
            inputActions.Player.Restart.canceled += context => onRestart.Value = false;

            inputActions.Player.Inventory.started += context => onInventory.Value = true;
            inputActions.Player.Inventory.canceled += context => onInventory.Value = false;

            //方向入力
            inputActions.Player.MoveUp.started += context => onMove.Value = ActorDirection.Up;
            inputActions.Player.MoveDown.started += context => onMove.Value = ActorDirection.Down;
            inputActions.Player.MoveLeft.started += context => onMove.Value = ActorDirection.Left;
            inputActions.Player.MoveRight.started += context => onMove.Value = ActorDirection.Right;
            inputActions.Player.MoveUpLeft.started += context => onMove.Value = ActorDirection.UpLeft;
            inputActions.Player.MoveUpRight.started += context => onMove.Value = ActorDirection.UpRight;
            inputActions.Player.MoveDownLeft.started += context => onMove.Value = ActorDirection.DownLeft;
            inputActions.Player.MoveDownRight.started += context => onMove.Value = ActorDirection.DownRight;

            //待機入力
            inputActions.Player.Wait.started += context => onWait.Value = true;
            inputActions.Player.Wait.canceled += context => onWait.Value = false;

            //デバッグ入力
            inputActions.Player.Debug.started += context => onDebug.Value = true;
            inputActions.Player.Debug.canceled += context => onDebug.Value = false;

        }

        public static void ClearDirection()
        {
            onMove.Value = ActorDirection.None;
        }

        /// <summary>
        /// 操作入力を許可する。
        /// </summary>
        public static void EnableInputs()
        {
            inputActions.Enable();
        }

        /// <summary>
        /// 操作入力を禁止する。
        /// </summary>
        public static void DisableInputs()
        {
            inputActions.Disable();
        }



        /// <summary>
        /// 軸入力を使いやすい値に変換。
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        private static Vector2 AxisInputNormalize(Vector2 rawValue)
        {
            //上下反転設定の適用。
            if (InvertYAxis)
            {
                rawValue.y *= -1f;
            }

            //操作感度反映。
            rawValue *= MouseLookSensitivity;

#if UNITY_WEBGL
            //WebGLビルドの場合、マウス感度をさらに下げておく。
            rawValue *= 0.25f;
#endif

            return rawValue;
        }
    }
}