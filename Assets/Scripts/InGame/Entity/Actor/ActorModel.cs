using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// ActorのModel。ゲーム上の実体。
    /// ステータスを管理し、行動を実行する。
    /// </summary>
    public class ActorModel : IActor
    {
        public string Name => Status.Name;
        public Vector2Int Position { get; private set; }
        public ActorType ActorType { get; }
        public IReadOnlyReactiveProperty<bool> IsDied => Status.IsDied;
        public ActorStatus Status { get; }

        public ActorModel(IActorProfile profile, Vector2Int position)
        {
            Status = new ActorStatus(profile);
            Position = position;
        }

        public void WarpTo(Vector2Int position) => Position = position;
        public void GetDamage(IActor dealer, AttackStatus attack) => Status.GetDamage(dealer, attack);
        public void Heal(int amount) => Status.Heal(amount);
        public void Die() => Status.Die();

        /// <summary>
        /// 指定したActorに攻撃する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="ad"></param>
        public void Attack(IActor target)
        {
            var attack = Status.PickAttackStatus();
            target.GetDamage(this, attack);
        }

        /// <summary>
        /// 指定方向へ移動する。
        /// MEMO:ここでは地形判定とかはしてない。Presenter側で行っている。
        /// </summary>
        /// <param name="direction"></param>
        public void StepTo(Vector2Int position)
        {
            Position = position;
        }

        /// <summary>
        /// 現在地点から指定した方向に進んだ地点を返す。
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector2Int GetPositionWithDirection(ActorDirection direction)
        {
            return Position + DirectionToVector(direction);
        }

        /// <summary>
        /// Directionをベクトルに変換して返す。
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector2Int DirectionToVector(ActorDirection direction)
        {
            switch (direction)
            {
                case ActorDirection.None: return Vector2Int.zero;
                case ActorDirection.Up: return Vector2Int.up;
                case ActorDirection.Down: return Vector2Int.down;
                case ActorDirection.Right: return Vector2Int.right;
                case ActorDirection.Left: return Vector2Int.left;
                case ActorDirection.UpRight: return new Vector2Int(1, 1);
                case ActorDirection.UpLeft: return new Vector2Int(-1, 1);
                case ActorDirection.DownRight: return new Vector2Int(1, -1);
                case ActorDirection.DownLeft: return new Vector2Int(-1, -1);
                default: throw new InvalidOperationException();
            }
        }
    }
}