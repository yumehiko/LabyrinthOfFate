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
    public class EntitySpawnPoints : IReadOnlyCollection<EntitySpawnPoint>
    {
        public int Count => points.Count;

        [SerializeField] private List<EntitySpawnPoint> points;

        public EntitySpawnPoints(List<EntitySpawnPoint> points)
        {
            this.points = points;
        }

        public IEnumerator<EntitySpawnPoint> GetEnumerator()
        {
            foreach (var point in points)
            {
                yield return point;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}