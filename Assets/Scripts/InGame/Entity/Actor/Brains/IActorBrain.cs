using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System;
using UniRx;

namespace yumehiko.LOF.Presenter
{
    /// <summary>
    /// Actorのターン挙動を決める。
    /// </summary>
    public interface IActorBrain
    {
        UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token);
    }
}