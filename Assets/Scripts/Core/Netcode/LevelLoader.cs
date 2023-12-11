using System.Linq;
using Core.Extensions;
using EventChannels.Runtime.Additions.Ids;
using Events.Runtime.Channels.Helpers;
using Unity.Netcode;
using UnityEngine;

namespace Core.Netcode
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private IdChannelSo channel;
        [SerializeField] private Id defaultLevelId;
        [SerializeField] private Id[] possibleIds;

        public void LoadDefault()
        {
            LoadServerRPC(defaultLevelId.name);
        }

        [ServerRpc(RequireOwnership = false)]
        private void LoadServerRPC(string idName)
        {
            LoadClientRPC(idName);
        }

        [ClientRpc]
        private void LoadClientRPC(string idName)
        {
            var id = possibleIds.FirstOrDefault(id => id.name == idName);
            if (id)
                channel.TryRaiseEvent(id);
            else
                this.LogError($"no {nameof(id)} provided with name {idName} in {nameof(possibleIds)}");
        }
    }
}
