using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Menus
{
    public class HostButton : ButtonBehaviour
    {
        protected override void HandleButtonClick()
        {
            Debug.Log(NetworkManager.Singleton.IsHost);
            NetworkManager.Singleton.StartHost();
        }
    }
}