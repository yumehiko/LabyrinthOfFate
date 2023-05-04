using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoF.GameLogic.Dungeon.Material
{
    /// <summary>
    ///     SpawnPointのコレクション。
    /// </summary>
    [Serializable]
    public class ActorSpawnPoints : IReadOnlyCollection<ActorSpawnPoint>
    {
        [SerializeField] private List<ActorSpawnPoint> points;

        public ActorSpawnPoints(List<ActorSpawnPoint> points)
        {
            this.points = points;
        }

        public int Count => points.Count;

        public IEnumerator<ActorSpawnPoint> GetEnumerator()
        {
            foreach (var point in points) yield return point;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}