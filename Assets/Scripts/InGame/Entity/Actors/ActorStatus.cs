using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    [System.Serializable]
	public class ActorStatus
	{
        public string Name => name;
        public int HP => hp;
        public int AD => ad;

        [SerializeField] private string name;
        [SerializeField] private int hp;
        [SerializeField] private int ad;
    }
}