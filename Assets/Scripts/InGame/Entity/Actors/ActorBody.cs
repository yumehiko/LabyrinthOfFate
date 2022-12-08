using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.LOF
{
	/// <summary>
    /// Actorの移動以外の行動を表す。
    /// </summary>
	public class ActorBody
	{
        public IReadOnlyReactiveProperty<int> HP => hp;

        private ActorStatus status;
        private IntReactiveProperty hp;

        public ActorBody(ActorStatus status)
        {
            this.status = status;
            hp = new IntReactiveProperty(status.HP);
        }

		/// <summary>
        /// 指定したActorに攻撃する。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="ad"></param>
		public void Attack(ActorBody target)
        {
            target.GetDamage(this, status.AD);
        }

        /// <summary>
        /// ダメージを受ける。
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="ad"></param>
        public void GetDamage(ActorBody dealer, int ad)
        {
            //MEMO: なんか跳ね返したりする場合もあるし、攻撃者はメモっておきたいが現状は使わない。
        }
	}
}