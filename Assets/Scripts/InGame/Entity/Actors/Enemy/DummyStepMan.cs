using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace yumehiko.LOF
{
    /// <summary>
    /// 毎ターンランダムに移動する。
    /// </summary>
	public class DummyStepMan : ActorBrain
    {
        public override Affiliation Affiliation => Affiliation.Enemy;
        public override ActorStatus Status => status;

        [SerializeField] private GridMovement gridMovement;

        private ActorVisual visual;
        private ActorStatus status;

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public override async UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token)
        {
            await Move(animationSpeedFactor, token);
        }

        public override void SetProfile(ActorStatus status, ActorVisual visual)
        {
            this.status = status;
            this.visual = visual;
        }

        private async UniTask Move(float animationSpeedFactor, CancellationToken token)
        {
            //このままだと攻撃はできない。
            EntityType entityType;
            ActorDirection direction;
            do
            {
                direction = GetRandomDirection();
                IEntity entity = gridMovement.CheckEntityTo(direction);
                entityType = entity == null ? EntityType.None : entity.EntityType;
            } while (entityType != EntityType.None);

            Vector2 endPoint = gridMovement.StepTo(direction);
            await visual.StepAnimation(endPoint, animationSpeedFactor).ToUniTask(cancellationToken: token);
        }

        private ActorDirection GetRandomDirection()
        {
            var enumArray = System.Enum.GetValues(typeof(ActorDirection));
            var randomID = Random.Range(0, enumArray.Length);
            return (ActorDirection)enumArray.GetValue(randomID);
        }
    }
}