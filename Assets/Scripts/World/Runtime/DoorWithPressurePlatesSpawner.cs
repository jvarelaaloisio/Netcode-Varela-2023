using System.Collections.Generic;
using System.Linq;
using Core.World;
using UnityEngine;

namespace World.Runtime
{
    public class DoorWithPressurePlatesSpawner : Spawner
    {
        [Header("Prefabs")]
        [SerializeField] private Door doorPrefab;
        [SerializeField] private PressurePlate pressurePlatePrefab;
        
        [Header("Positions")]
        [SerializeField] private Transform doorSpawnPoint;
        [SerializeField] private List<Transform> pressurePlateSpawnPoint;

        public override void Spawn()
        {
            if (!doorPrefab)
            {
                Debug.LogError($"[{name}] {nameof(doorPrefab)} is null!");
                return;
            }

            var door = Instantiate(doorPrefab, doorSpawnPoint.position, doorSpawnPoint.rotation);
            SpawnNetworkObject(door);

            if (!pressurePlatePrefab)
            {
                Debug.LogError($"{name}: {nameof(pressurePlatePrefab)} is null!");
                return;
            }
            
            var notNullSpawnPoints = pressurePlateSpawnPoint.Where(t => t);
            foreach (var spawnPoint in notNullSpawnPoints)
            {
                var pressurePlate = Instantiate(pressurePlatePrefab, spawnPoint.position, spawnPoint.rotation);
                SpawnNetworkObject(pressurePlate);
                door.AddPressurePlate(pressurePlate);
            }
        }
    }
}