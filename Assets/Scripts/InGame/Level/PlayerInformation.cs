using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.LOF
{
    public class PlayerInformation : MonoBehaviour
    {
        public Player PlayerPrefab => playerPrefab;
        public ActorVisual PlayerVisualPrefab => playerVisualPrefab;
        public ActorStatus PlayerStatus => playerStatus;

        [SerializeField] private Player playerPrefab;
        [SerializeField] private ActorVisual playerVisualPrefab;
        [SerializeField] private ActorStatus playerStatus;
    }
}