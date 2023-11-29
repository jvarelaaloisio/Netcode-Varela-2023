using System.Collections;
using System.Collections.Generic;
using Core.World;
using UnityEngine;

namespace Management
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<Spawner> spawners;

        public IEnumerator Init()
        {
            foreach (var spawner in spawners)
            {
                spawner.Spawn();
            }

            yield break;
        }
    }
}