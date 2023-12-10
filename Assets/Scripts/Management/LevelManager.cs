using System.Collections;
using System.Collections.Generic;
using Core.Characters;
using Core.Extensions;
using Core.Models;
using Core.World;
using Unity.Netcode;
using UnityEngine;

namespace Management
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<Spawner> spawners;
        [SerializeField] private Transform[] playerSpawnPoints;
        [SerializeField] private CharacterSetup[] playerSetups;

        [Range(1, 100)] [Tooltip("The max quantity of spawners to spawn every frame")] [SerializeField]
        private int spawnBatchSize = 10;


        public IEnumerator InitAsHost()
        {
            int counter = 0;
            foreach (var spawner in spawners)
            {
                spawner.Spawn();
                if (++counter >= spawnBatchSize)
                    yield return null;
            }
        }

        public IEnumerator SetupPlayer(NetworkObject newPlayer)
        {
            var clientId = (int)newPlayer.OwnerClientId;
            this.Log($"Setting up player {clientId}");
            var setup = newPlayer.GetComponentInChildren<ICharacterSetup>();
            if (setup == null)
                this.LogError($"{nameof(setup)} is null! (clientId was {clientId})");
            else if (playerSetups.TryGetValueNotNull(clientId, out var characterSetup))
            {
                setup.OverrideBaseSprite(characterSetup.BaseSprite);
                setup.OverrideAnimatorController(characterSetup.AnimatorOverride);
            }

            if (playerSpawnPoints.TryGetValueNotNull(clientId, out var spawnPoint))
                newPlayer.transform.position = spawnPoint.position;
            yield break;
        }
    }
}