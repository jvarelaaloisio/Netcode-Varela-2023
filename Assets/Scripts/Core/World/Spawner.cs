using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        protected IEnumerable<T> SpawnList<T>(SpawnConfig<T> configs) where T : MonoBehaviour
        {
            var result = new List<T>();
            var notNullSpawnPoints = Enumerable.Where<Transform>(configs.SpawnPoints, t => t);
            foreach (var spawnPoint in notNullSpawnPoints)
            {
                var spawned = Instantiate<T>(configs.Prefab, spawnPoint.position, spawnPoint.rotation);
                SceneManager.MoveGameObjectToScene(spawned.gameObject, gameObject.scene);
                SpawnNetworkObject(spawned);
                result.Add(spawned);
            }

            return result;
        }
    }
}