using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    public class CollTest : MonoBehaviour, IEntity
    {
        public EntityType EntityType => EntityType.Actor;
    }
}