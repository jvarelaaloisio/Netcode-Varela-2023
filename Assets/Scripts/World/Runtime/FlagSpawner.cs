using UnityEngine;

using Core.Extensions;
using Core.World;

namespace World.Runtime
{
    public class FlagSpawner : Spawner
    {
        [SerializeField] private Flag.Config config;
        public override void Spawn()
        {
            if (config.Prefab == null)
            {
                this.LogError($"{nameof(config.Prefab)} is null!");
                return;
            }

            var flags = SpawnList(config);
            foreach (var flag in flags)
            {
                flag.ApplyConfig(config);
            }
        }
    }
}
