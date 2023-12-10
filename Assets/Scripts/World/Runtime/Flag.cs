using System;
using System.Linq;
using Core.Extensions;
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
        [field: SerializeField] public IdChannelSo GoToNextLevelChannel { get; set; }
        [field: SerializeField] public Id NextLevelId { get; set; }
        [SerializeField] private Animator animator;
        [SerializeField] private string turnOnTriggerParam;
        [SerializeField] private Id[] possibleIds;

        public void ApplyConfig(Config config)
        {
            TurnOnId = config.TurnOnIdFilter;
            TurnOnChannel = config.TurnOnChannel;
            TurnOnChannel.TrySubscribe(TurnOn);
            
            NextLevelId = config.NextLevelId;
            GoToNextLevelChannel = config.GoToNextLevelChannel;
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

        private void OnTriggerEnter2D(Collider2D col)
        { 
            RaiseNextLevelServerRPC();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RaiseNextLevelServerRPC()
        {
            RaiseNextLevelClientRPC(NextLevelId.name);
        }

        [ClientRpc]
        private void RaiseNextLevelClientRPC(string idName)
        {
            var id = possibleIds.FirstOrDefault(id => id.name == idName);
            if (id)
                GoToNextLevelChannel.TryRaiseEvent(id);
            else
                this.LogError($"no {nameof(id)} provided with name {idName} in {nameof(possibleIds)}");
        }

        [Serializable]
        public class Config : SpawnConfig<Flag>
        {
            [field: SerializeField] public IdChannelSo TurnOnChannel { get; set; }
            [field: SerializeField] public Id TurnOnIdFilter { get; set; }
            [field: SerializeField] public IdChannelSo GoToNextLevelChannel { get; set; }
            [field: SerializeField] public Id NextLevelId { get; set; }
        }
    }
}