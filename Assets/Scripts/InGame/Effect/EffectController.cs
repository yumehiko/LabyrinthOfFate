using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace yumehiko.LOF.View
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField] private Transform effectsParent;
        [SerializeField] private BuffEffect buffEffectPrefab;

        public async UniTask DoBuffEffect(Vector2Int position, ActRequest request)
            => await buffEffectPrefab.DoEffect(position, effectsParent, request);
    }
}