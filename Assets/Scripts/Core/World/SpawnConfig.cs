using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.World
{
    [Serializable]
    public class SpawnConfig<T> where T : MonoBehaviour
    {
        [field: Header("Prefabs")]
        [field: SerializeField] public T Prefab { get; set; }

        [field: Header("Positions")]
        [field: SerializeField] public List<Transform> SpawnPoints { get; set; }
    }
}