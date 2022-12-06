using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    /// <summary>
    /// レベル。攻略が要求される1つの単位。フロア、階層。
    /// プレイヤー、地形、敵などのエンティティから成る。
    /// </summary>
	public class Level : MonoBehaviour
	{
		[SerializeField] private EntitySpawner entitySpawner;
        [SerializeField] private Turn turn;

        private void Awake()
        {
            entitySpawner.SpawnEntities();
            turn.Startup(entitySpawner.Players, entitySpawner.Enemies);
        }
    }
}