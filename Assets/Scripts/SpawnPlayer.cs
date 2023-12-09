using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Obsolete]
public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private NetworkObject player;
    
    [ContextMenu("Spawn player")]
    private void SpawnPlayerCharacter()
    {
        var newChar = Instantiate(player);
        newChar.Spawn(true);
    }
}
