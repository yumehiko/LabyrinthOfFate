using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    /// <summary>
    /// Actorが生成されるポイント。
    /// </summary>
    public class SpawnPoint : MonoBehaviour, ISpawnPoint
    {
        [SerializeField] private Affiliation affiliation;
        public Affiliation Affiliation => affiliation;
        public Vector2 Position => transform.position;
    }
}