using Cysharp.Threading.Tasks;
using LoF.GameLogic.Entity.Actor.Model;

namespace LoF.GameLogic.Entity.Actor.Brains
{
    /// <summary>
    ///     経路を探索し、Playerに向かって進む。
    /// </summary>
    public class PathFindMelee : ActorBrainBase
    {
        private readonly Level level;

        public PathFindMelee(Level level, IActorModel model, IActorView view)
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
                //インベントリにカードがあるなら、それをInvokeする。
                if (Model.Inventory.Count > 0)
                {
                    Model.Inventory.InvokeCard(0, Model);
                    await View.InvokeAnimation(Model.Position, request);
                    return;
                }

                await StepOrAttack(request);
            }
            finally
            {
                Model.Status.UseEnergy();
            }
        }

        /// <summary>
        ///     移動または攻撃
        /// </summary>
        /// <param name="animationSpeedFactor"></param>
        /// <param name="logicCT"></param>
        /// <returns></returns>
        private async UniTask StepOrAttack(ActRequest request)
        {
            var start = Model.Position;
            var end = level.Actors.GetPlayerPosition();
            var path = level.Terrain.FindPath(start, end);

            //経路がない場合は、その場で待機。
            if (path.Length <= 1)
            {
                await View.WaitAnimation(start, request);
                return;
            }

            //経路番号1がPlayerの位置なら、隣接しているので攻撃する。
            if (path[1] == level.Actors.GetPlayerPosition())
            {
                Model.Attack(level.Actors.Player.Model);
                await View.AttackAnimation(path[1], request);
                return;
            }

            //移動先にPlayer以外のActorがいる場合、単に停止する。
            if (level.Actors.GetEnemyAt(path[1]) != null)
            {
                await View.WaitAnimation(start, request);
                return;
            }

            //それ以外の場合、経路1へ移動する。
            Model.StepTo(path[1]);
            await View.StepAnimation(path[1], request);
        }
    }
}