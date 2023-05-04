using Cysharp.Threading.Tasks;
using LoF.GameLogic.Entity.Actor;
using UnityEngine;

namespace LoF.Effect
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField] private Transform effectsParent;
        [SerializeField] private BuffEffect buffEffectPrefab;

        public async UniTask DoBuffEffect(Vector2Int position, ActRequest request)
        {
            await buffEffectPrefab.DoEffect(position, effectsParent, request);
        }
    }
}