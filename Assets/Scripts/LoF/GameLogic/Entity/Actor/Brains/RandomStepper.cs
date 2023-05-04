using System;
using Cysharp.Threading.Tasks;
using LoF.GameLogic.Dungeon.Material;
using LoF.GameLogic.Entity.Actor.Model;
using Random = UnityEngine.Random;

namespace LoF.GameLogic.Entity.Actor.Brains
{
    /// <summary>
    ///     毎ターンランダムに移動する。
    /// </summary>
    public class RandomStepper : ActorBrainBase
    {
        private readonly Level level;

        public RandomStepper(Level level, IActorModel model, IActorView view)
        {
            this.level = level;
            Model = model;
            View = view;
        }

        public override IActorModel Model { get; }
        public override IActorView View { get; }

        /// <summary>
        ///     ターンアクションを実行する。
        /// </summary>
        public override async UniTask DoTurnAction(ActRequest request)
        {
            try
            {
                await RandomStep(request);
            }
            finally
            {
                Model.Status.UseEnergy();
            }
        }

        private async UniTask RandomStep(ActRequest request)
        {
            while (true)
            {
                var direction = GetRandomDirection();
                var point = Model.GetPositionWithDirection(direction);

                //方向指定がない場合は、その場で待機。
                if (direction == ActorDirection.None)
                {
                    await View.WaitAnimation(point, request);
                    return;
                }

                //指定地点にActorがいるか確認する。
                var actor = level.Actors.GetActorAt(point);

                //それがプレイヤーなら攻撃する。
                if (actor != null && actor.ActorType == ActorType.Player)
                {
                    Model.Attack(actor);
                    await View.AttackAnimation(point, request);
                    return;
                }

                //それがプレイヤー以外なら、行動入力へ戻る。
                if (actor != null) continue;

                //Actorがいない上に、地形が空なら移動する。
                if (level.Terrain.GetTileType(point) == TileType.Empty)
                {
                    Model.StepTo(point);
                    await View.StepAnimation(point, request);
                    return;
                }
            }
        }

        private ActorDirection GetRandomDirection()
        {
            var enumArray = Enum.GetValues(typeof(ActorDirection));
            var randomID = Random.Range(0, enumArray.Length);
            return (ActorDirection)enumArray.GetValue(randomID);
        }
    }
}