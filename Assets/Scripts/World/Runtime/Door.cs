using System;
using System.Collections;
using System.Collections.Generic;
using Core.Extensions;
using Core.World;
using Unity.Netcode;
using UnityEngine;

namespace World.Runtime
{
    public class Door : NetworkBehaviour
    {
        [Header("Setup")]
        [SerializeField] private float openDuration = 1;

        [field: SerializeField] public Vector3 MoveOffset { get; set; } = Vector3.up;

        [Header("Animator")]
        [SerializeField] private Animator animator;
        [SerializeField] private string moveAnimParam = "isMoving";
        private int moveAnimParamHash;

        public Action<Door> onOpen;

        public int PlatesNeeded { get; private set; }
        [field: SerializeField] public List<PressurePlate> PressurePlates { get; set; }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            moveAnimParamHash = Animator.StringToHash(moveAnimParam);
        }

        /// <summary>
        /// Let's setup this spawnable
        /// </summary>
        /// <param name="config"></param>
        /// <param name="plates">Pressure plates to add</param>
        /// <param name="onOpenHandler">Handler method for <see cref="onOpen"/>> callback</param>
        public void Init(Config config, IEnumerable<PressurePlate> plates, Action<Door> onOpenHandler = null)
        {
            MoveOffset = config.MoveOffset;
            foreach (var plate in plates)
                AddPressurePlate(plate);
            if (onOpenHandler != null)
                onOpen += onOpenHandler;
        }
        
        public void AddPressurePlate(PressurePlate pressurePlate)
        {
            pressurePlate.OnPress += HandlePress;
            PlatesNeeded++;
        }

        private void HandlePress(PressurePlate pressurePlate)
        {
            PlatesNeeded--;
            pressurePlate.OnPress -= HandlePress;
            if (PlatesNeeded <= 0)
                StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            if (animator)
                animator.SetBool(moveAnimParamHash, true);
            yield return MoveTowards(MoveOffset);
            onOpen?.Invoke(this);
            if (animator)
                animator.SetBool(moveAnimParamHash, false);
            this.Log($"moved towards {MoveOffset}!");
        }

        private IEnumerator MoveTowards(Vector3 offset)
        {
            var startPosition = transform.position;
            var destPosition = startPosition + offset;
            var startTime = Time.time;
            var now = Time.time;
            while (startTime + openDuration > now)
            {
                var lerp = (now - startTime) / openDuration;
                transform.position = Vector3.Lerp(startPosition, destPosition, lerp);
                yield return null;
                now = Time.time;
            }

            transform.position = destPosition;
        }
        [Serializable]
        public class Config : SpawnConfig<Door>
        {
            [field: SerializeField] public Vector3 MoveOffset { get; set; } = Vector3.up;
        }
    }
}