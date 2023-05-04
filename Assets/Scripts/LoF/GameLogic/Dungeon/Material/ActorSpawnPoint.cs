using System;
using LoF.GameLogic.Entity.Actor.Model;
using UnityEngine;

namespace LoF.GameLogic.Dungeon.Material
{
    /// <summary>
    ///     Actorが発生するポイント。
    /// </summary>
    [Serializable]
    public struct ActorSpawnPoint
    {
        [SerializeField] private Vector2Int position;
        [SerializeField] private ActorType type;

        public ActorSpawnPoint(Vector2Int position, ActorType type)
        {
            this.position = position;
            this.type = type;
        }

        public ActorSpawnPoint(short x, short y, ActorType type)
        {
            position = new Vector2Int(x, y);
            this.type = type;
        }

        public Vector2Int Position => position;
        public ActorType Type => type;
    }
}