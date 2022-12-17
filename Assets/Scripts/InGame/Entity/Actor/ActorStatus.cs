using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace yumehiko.LOF.Model
{
    /// <summary>
    /// あるActorの持つステータス。
    /// </summary>
    [Serializable]
	public class ActorStatus
	{
        public BrainType BrainType => brainType;
        public string Name => name;
        public int HP => hp;
        public int AD => ad; //仮。装備品などで定義する。

        [SerializeField] private BrainType brainType;
        [SerializeField] private string name;
        [SerializeField] private int hp;
        [SerializeField] private int ad;
    }
}