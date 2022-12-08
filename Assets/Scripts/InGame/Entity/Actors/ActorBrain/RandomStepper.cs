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
	public class RandomStepper : ActorBrain
    {
        public override Affiliation Affiliation => Affiliation.Enemy;

        [SerializeField] private GridMovement gridMovement;

        /// <summary>
        /// ターンアクションを実行する。
        /// </summary>
        public override async UniTask DoTurnAction(float animationSpeedFactor, CancellationToken token)
        {
            await Move(animationSpeedFactor, token);
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