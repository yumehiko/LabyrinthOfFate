﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// ダンジョンを表すデータ。
    /// フロアを持ち、エンティティが配置され、ゲームの基盤となる。
    /// </summary>
    [Serializable]
    public class Dungeon
    {
        [SerializeField] private Floor floor;
        [SerializeField] private EntitySpawnPoints entitySpawnPoints;
        [SerializeField] private BoundsInt bounds;

        public Floor Floor => floor;
        public EntitySpawnPoints EntitySpawnPoints => entitySpawnPoints;
        public BoundsInt Bounds => bounds;

        public Dungeon(Floor floor, EntitySpawnPoints entitySpawnPoints, BoundsInt bounds)
        {
            this.floor = floor;
            this.entitySpawnPoints = entitySpawnPoints;
            this.bounds = bounds;
        }
    }
}