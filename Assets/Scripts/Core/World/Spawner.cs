using Unity.Netcode;
using UnityEngine;

namespace Core.World
{
    public abstract class Spawner : MonoBehaviour
    {
        public abstract void Spawn();

        protected void SpawnNetworkObject(Component instantiatedObject)
        {
            if (instantiatedObject.TryGetComponent(out NetworkObject doorNetworkObject))
            {
                doorNetworkObject.Spawn();
            }
            else
                Debug.LogError($"[{name}] {instantiatedObject.name} doesn't have networkObject component!");
        }
    }
}