using System;

using UnityEngine;
using Unity.Netcode;

using Core.World;
using EventChannels.Runtime.Additions.Ids;
using Events.Runtime.Channels.Helpers;

namespace World.Runtime
{
    public class Flag : NetworkBehaviour
    {
        [field: SerializeField] public IdChannelSo TurnOnChannel { get; set; }
        [field: SerializeField] public Id TurnOnId { get; set; }
        [SerializeField] private Animator animator;
        [SerializeField] private string turnOnTriggerParam;

        public void Init(Config config)
        {
            TurnOnChannel = config.TurnOnChannel;
            TurnOnChannel.TrySubscribe(TurnOn);
            TurnOnId = config.ID;
        }
        
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            TurnOnChannel.TryUnsubscribe(TurnOn);
        }

        private void TurnOn(Id sentId)
        {
            if (sentId == TurnOnId)
            {
                animator.SetTrigger(turnOnTriggerParam);
            }
        }
        [Serializable]
        public class Config : SpawnConfig<Flag>
        {
            [field: SerializeField] public IdChannelSo TurnOnChannel { get; set; }
            [field: SerializeField] public Id ID { get; set; }
        }
    }
}