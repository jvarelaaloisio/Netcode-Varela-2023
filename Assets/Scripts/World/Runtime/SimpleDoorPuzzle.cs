using System.Linq;

using UnityEngine;

using Core.World;
using EventChannels.Runtime.Additions.Ids;

namespace World.Runtime
{
    public class SimpleDoorPuzzle : Spawner
    {
        [SerializeField] private Door.Config doorConfig;
        [SerializeField] private SpawnConfig<PressurePlate> pressurePlateConfig;
        
        [Header("On Complete")]
        [SerializeField] private Id id;
        [SerializeField] private IdChannelSo onCompleteChannel;
        
        public override void Spawn()
        {
            if (!doorConfig.Prefab)
            {
                Debug.LogError($"[{name}] {nameof(doorConfig.Prefab)} is null!");
                return;
            }

            var doors = SpawnList(doorConfig).ToArray();

            if (!pressurePlateConfig.Prefab)
            {
                Debug.LogError($"{name}: {nameof(pressurePlateConfig.Prefab)} is null!");
                return;
            }

            var pressurePlates = SpawnList(pressurePlateConfig).ToArray();
            
            foreach (var door in doors)
                door.Init(doorConfig, pressurePlates);
            
            var firstDoor = doors.FirstOrDefault();
            if (firstDoor)
                firstDoor.onOpen += HandleDoorOpened;
        }

        private void HandleDoorOpened(Door door)
        {
            onCompleteChannel.RaiseEvent(id);
        }

#if UNITY_EDITOR
        [ContextMenu("Clean Up")]
        private void CleanUp()
        {
            doorConfig.SpawnPoints.RemoveAll(t => t == null);
            pressurePlateConfig.SpawnPoints.RemoveAll(t => t == null);
        }
#endif
    }
}