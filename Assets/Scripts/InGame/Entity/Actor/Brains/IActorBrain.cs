using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System;
using UniRx;
using yumehiko.LOF.Model;
using yumehiko.LOF.View;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// Actorのターン挙動を決める。
    /// MEMO:現状、プレゼンターの役割もしているが分割すべきかどうか。
    /// </summary>
    public interface IActorBrain
    {
        IActorModel Model { get; }
        IActorView View { get; }
        UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token);
        void WarpTo(Vector2Int position);
        void Heal(int amount);
    }
}