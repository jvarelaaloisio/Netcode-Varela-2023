using System;
using System.Collections;
using System.Collections.Generic;
using Events.Runtime.Channels;
using UnityEngine;

namespace Menus
{
    [Obsolete]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private VoidChannelSo hostChannel;
        [SerializeField] private VoidChannelSo joinChannel;

        private void Awake()
        {
            
        }
    }
}
