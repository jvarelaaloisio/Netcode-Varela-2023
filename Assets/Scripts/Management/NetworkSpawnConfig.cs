using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Management
{
    [Serializable]
    public struct NetworkSpawnConfig
    {
        [field: SerializeField] public Transform Parent { get; set; }
        [field: SerializeField] public NetworkObject Prefab { get; set; }

        public void Spawn(Scene scene)
        {
            var spawned = Object.Instantiate(Prefab, Parent.position, Parent.rotation);
            spawned.Spawn(true);
            SceneManager.MoveGameObjectToScene(spawned.gameObject, scene);
        }
    }
}