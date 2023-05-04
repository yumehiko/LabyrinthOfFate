using Cysharp.Threading.Tasks;
using LoF.GameLogic.Entity.Actor.Model;
using UnityEngine;

namespace LoF.GameLogic.Entity.Actor.Brains
{
    /// <summary>
    ///     Actorのターン挙動を決める。
    ///     MEMO:現状、プレゼンターの役割もしているが分割すべきかどうか。
    /// </summary>
    public interface IActorBrain
    {
        IActorModel Model { get; }
        IActorView View { get; }
        bool HasEnergy { get; }
        void RefleshEnergy();
        UniTask DoTurnAction(ActRequest request);
        void WarpTo(Vector2Int position);
        void Heal(int amount);
        void Destroy();
    }
}