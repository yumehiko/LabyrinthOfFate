using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    /// <summary>
    /// ColliderによってActorだと教えるためだけのコンポーネント……大変馬鹿っぽい。
    /// </summary>
    public class ActorBody : MonoBehaviour, IEntity
    {
        public EntityType EntityType => EntityType.Actor;
    }
}