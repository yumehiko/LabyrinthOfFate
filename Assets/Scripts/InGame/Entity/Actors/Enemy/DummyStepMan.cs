using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace yumehiko.LOF
{
    /// <summary>
    /// 毎ターンランダムに移動する。
    /// </summary>
	public class DummyStepMan : ActorBrain
	{
		[SerializeField] private GridMovement gridMovement;
        [SerializeField] private ActorVisual visual;

        public override Affiliation Affiliation => Affiliation.Enemy;

		/// <summary>
        /// ターンアクションを実行する。
        /// </summary>
		public override async UniTask DoTurnAction(float timeFactor)
        {
            await Move(0.25f);
        }

        private async UniTask Move(float duration)
        {
            //このままだと攻撃はできない。
            var entityType = EntityType.None;
            var direction = ActorDirection.None;
            while (entityType != EntityType.None)
            {
                direction = GetRandomDirection();
                IEntity entity = gridMovement.CheckEntityTo(direction);
                entityType = entity == null ? EntityType.None : entity.EntityType;
            }

            Vector2 endPoint = gridMovement.StepTo(direction);
            await visual.StepAnimation(endPoint, duration);
        }

        private ActorDirection GetRandomDirection()
        {
            var enumArray = System.Enum.GetValues(typeof(ActorDirection));
            var randomID = Random.Range(0, enumArray.Length);
            return (ActorDirection)enumArray.GetValue(randomID);
        }
    }
}