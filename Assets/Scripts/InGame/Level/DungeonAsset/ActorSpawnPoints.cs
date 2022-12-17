using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// SpawnPointのコレクション。
    /// </summary>
    [Serializable]
    public class ActorSpawnPoints : IReadOnlyCollection<ActorSpawnPoint>
    {
        public int Count => points.Count;

        [SerializeField] private List<ActorSpawnPoint> points;

        public ActorSpawnPoints(List<ActorSpawnPoint> points)
        {
            this.points = points;
        }

        public IEnumerator<ActorSpawnPoint> GetEnumerator()
        {
            foreach (var point in points)
            {
                yield return point;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}