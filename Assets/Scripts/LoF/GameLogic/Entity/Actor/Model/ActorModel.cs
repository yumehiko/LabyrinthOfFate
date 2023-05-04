using System;
using LoF.GameLogic.Entity.Item.Card;
using LoF.UI.ResultMessage;
using UniRx;
using UnityEngine;

namespace LoF.GameLogic.Entity.Actor.Model
{
    /// <summary>
    ///     ActorのModel。ゲーム上の実体。
    ///     ステータスを管理し、行動を実行する。
    /// </summary>
    public class ActorModel : IActorModel
    {
        private readonly Subject<IActResult> onActResult = new();

        public ActorModel(IActorProfile profile, Vector2Int position, ActorType actorType)
        {
            ActorType = actorType;
            Inventory = new Inventory(profile);
            Status = new ActorStatus(profile, Inventory.EquipSlot);
            Position = position;
        }

        public string Name => Status.Name;
        public Vector2Int Position { get; private set; }
        public ActorType ActorType { get; }
        public ActorStatus Status { get; }
        public Inventory Inventory { get; }
        public IReadOnlyReactiveProperty<bool> IsDied => Status.IsDied;
        public IObservable<IActResult> OnActResult => onActResult;

        public void SendResultMessage(IActResult result)
        {
            onActResult.OnNext(result);
        }

        public void WarpTo(Vector2Int position)
        {
            Position = position;
        }

        public void Heal(int amount)
        {
            Status.Heal(amount);
        }

        public void Die()
        {
            Status.Die();
        }

        /// <summary>
        ///     指定したActorに攻撃する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="ad"></param>
        public void Attack(IActorModel target)
        {
            var attack = Status.PickAttackStatus();
            target.GetDamage(this, attack);
        }

        public void GetDamage(IActorModel dealer, AttackStatus attack)
        {
            var damage = new Damage(attack, Status.DefenceStatus);
            var result = new BattleResult(dealer, this, damage);
            onActResult.OnNext(result);
            Status.TakeDamage(damage);
        }

        /// <summary>
        ///     指定方向へ移動する。
        ///     MEMO:ここでは地形判定とかはしてない。Presenter側で行っている。
        /// </summary>
        /// <param name="direction"></param>
        public void StepTo(Vector2Int position)
        {
            Position = position;
        }

        /// <summary>
        ///     現在地点から指定した方向に進んだ地点を返す。
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector2Int GetPositionWithDirection(ActorDirection direction)
        {
            return Position + DirectionToVector(direction);
        }

        /// <summary>
        ///     Directionをベクトルに変換して返す。
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