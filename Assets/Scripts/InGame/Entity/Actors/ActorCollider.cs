using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    /// <summary>
    /// Actorだと教えるためだけのコンポーネント……大変馬鹿っぽい。
    /// ActorBodyなどとして、HP情報などをまとめるコンポーネントにしたらいいか？
    /// </summary>
    public class ActorCollider : MonoBehaviour, IEntity
    {
        public EntityType EntityType => EntityType.Actor;
    }
}