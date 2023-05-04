using LoF.GameLogic.Entity.Actor.Model;
using UniRx;
using UnityEngine;

namespace LoF.Input
{
    /// <summary>
    ///     プレイヤーの入力（コントローラーやキーボード）を監視・管理する。
    /// </summary>
    public static class ReactiveInput
    {
        private static UserInputs _inputActions;


        private static readonly Vector2ReactiveProperty onPointer = new();
        private static readonly Vector2ReactiveProperty onRightStick = new();
        private static readonly BoolReactiveProperty onMaru = new();
        private static readonly BoolReactiveProperty onPeke = new();
        private static readonly BoolReactiveProperty onPause = new();
        private static readonly BoolReactiveProperty onRestart = new();
        private static readonly BoolReactiveProperty onInventory = new();
        private static readonly ActorDirectionReactiveProperty onMove = new();
        private static readonly BoolReactiveProperty onWait = new();
        private static readonly BoolReactiveProperty onDebug = new();


        static ReactiveInput()
        {
            SubscribeInputs();
        }

        /// <summary>
        ///     方向操作入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<ActorDirection> OnMove => onMove;

        /// <summary>
        ///     待機入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnWait => onWait;

        /// <summary>
        ///     ポインター（マウス）操作の入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<Vector2> OnPointer => onPointer;

        /// <summary>
        ///     右スティック操作の入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<Vector2> OnRightStick => onRightStick;

        /// <summary>
        ///     マルボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnMaru => onMaru;

        /// <summary>
        ///     ペケボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnPeke => onPeke;

        /// <summary>
        ///     ポーズボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnPause => onPause;

        /// <summary>
        ///     リスタートボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnRestart => onRestart;

        /// <summary>
        ///     インベントリボタンの入力。
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnInventory => onInventory;

        /// <summary>
        ///     デバッグキー入力
        /// </summary>
        public static IReadOnlyReactiveProperty<bool> OnDebug => onDebug;


        private static void SubscribeInputs()
        {
            //inputActionを生成
            _inputActions = new UserInputs();
            _inputActions.Enable();

            //各操作入力のON/OFFを購読。
            _inputActions.Player.Maru.started += context => onMaru.Value = true;
            _inputActions.Player.Maru.canceled += context => onMaru.Value = false;

            _inputActions.Player.Peke.started += context => onPeke.Value = true;
            _inputActions.Player.Peke.canceled += context => onPeke.Value = false;

            _inputActions.Player.Pointer.performed += context =>
                onPointer.Value = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());

            _inputActions.Player.RightStick.performed += context => onRightStick.Value = context.ReadValue<Vector2>();
            _inputActions.Player.RightStick.canceled += context => onRightStick.Value = context.ReadValue<Vector2>();

            _inputActions.Player.Pause.started += context => onPause.Value = true;
            _inputActions.Player.Pause.canceled += context => onPause.Value = false;

            _inputActions.Player.Restart.started += context => onRestart.Value = true;
            _inputActions.Player.Restart.canceled += context => onRestart.Value = false;

            _inputActions.Player.Inventory.started += context => onInventory.Value = true;
            _inputActions.Player.Inventory.canceled += context => onInventory.Value = false;

            //方向入力
            _inputActions.Player.MoveUp.started += context => onMove.Value = ActorDirection.Up;
            _inputActions.Player.MoveDown.started += context => onMove.Value = ActorDirection.Down;
            _inputActions.Player.MoveLeft.started += context => onMove.Value = ActorDirection.Left;
            _inputActions.Player.MoveRight.started += context => onMove.Value = ActorDirection.Right;
            _inputActions.Player.MoveUpLeft.started += context => onMove.Value = ActorDirection.UpLeft;
            _inputActions.Player.MoveUpRight.started += context => onMove.Value = ActorDirection.UpRight;
            _inputActions.Player.MoveDownLeft.started += context => onMove.Value = ActorDirection.DownLeft;
            _inputActions.Player.MoveDownRight.started += context => onMove.Value = ActorDirection.DownRight;

            //待機入力
            _inputActions.Player.Wait.started += context => onWait.Value = true;
            _inputActions.Player.Wait.canceled += context => onWait.Value = false;

            //デバッグ入力
            _inputActions.Player.Debug.started += context => onDebug.Value = true;
            _inputActions.Player.Debug.canceled += context => onDebug.Value = false;
        }

        public static void ClearDirection()
        {
            onMove.Value = ActorDirection.None;
        }

        /// <summary>
        ///     操作入力を許可する。
        /// </summary>
        public static void EnableInputs()
        {
            _inputActions.Enable();
        }

        /// <summary>
        ///     操作入力を禁止する。
        /// </summary>
        public static void DisableInputs()
        {
            _inputActions.Disable();
        }
    }
}