﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// Actorの行動を実行する。
    /// </summary>
    public class ActorBody : IActor
    {
        public IReadOnlyReactiveProperty<int> HP => hp;
        public Vector2Int Position { get; private set; }
        public ActorType ActorType { get; }
        public EntityType EntityType => EntityType.Actor;
        public IObservable<Unit> OnDie => onDie;
        public IObservable<Unit> OnStepStart => onStep;
        public IObservable<Unit> OnAttackStart => onAttack;

        private readonly ActorStatus status;
        private readonly IntReactiveProperty hp;
        private readonly Subject<Unit> onDie;
        private readonly Subject<Unit> onStep = new Subject<Unit>();
        private readonly Subject<Unit> onAttack = new Subject<Unit>();

        public ActorBody(ActorStatus status, Vector2Int position)
        {
            this.status = status;
            hp = new IntReactiveProperty(status.HP);
            onDie = new Subject<Unit>();
            Position = position;
        }

        public void Die() => onDie.OnNext(Unit.Default);

        /// <summary>
        /// 指定したActorに攻撃する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="ad"></param>
        public void Attack(IActor target)
        {
            target.GetDamage(this, status.AD);
            onAttack.OnNext(Unit.Default);
        }

        /// <summary>
        /// ダメージを受ける。
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="ad"></param>
        public void GetDamage(IActor dealer, int ad)
        {
            //MEMO: なんか跳ね返したりする場合もあるし、攻撃者はメモっておきたいが現状は使わない。
            hp.Value = Mathf.Max(hp.Value - ad, 0);
        }

        /// <summary>
        /// 指定方向へ移動する。
        /// TODO:地形判定とかはしてない。
        /// </summary>
        /// <param name="direction"></param>
        public void StepTo(Vector2Int position)
        {
            Position = position;
            onStep.OnNext(Unit.Default);
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