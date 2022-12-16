using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// エンティティが発生するポイント。
    /// </summary>
    [Serializable]
    public struct EntitySpawnPoint
    {
        public Vector2Int Position => position;
        public ActorType Type => type;

        [SerializeField] Vector2Int position;
        [SerializeField] private ActorType type;

        public EntitySpawnPoint(Vector2Int position, ActorType type)
        {
            this.position = position;
            this.type = type;
        }

        public EntitySpawnPoint(short x, short y, ActorType type)
        {
            this.position = new Vector2Int(x, y);
            this.type = type;
        }
    }
}