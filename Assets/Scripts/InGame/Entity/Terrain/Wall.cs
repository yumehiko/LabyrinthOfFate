using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    public class Wall : MonoBehaviour, IEntity
    {
        public EntityType EntityType => EntityType.Terrain;
    }
}