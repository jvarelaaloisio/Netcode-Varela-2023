using System;
using System.Collections.Generic;
using EventChannels.Runtime.Additions.Ids;
using Events.Runtime.Channels.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Management
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private List<Menu> menus;
        [SerializeField] private Id defaultMenuId;
        
        [Header("Channels listened")]
        [SerializeField] private IdChannelSo setMenuChannel;
        
        private void OnEnable()
        {
            if (defaultMenuId)
                SetMenu(defaultMenuId);
            else
                Debug.LogWarning($"{name}: No default Menu ID set");

            if(!setMenuChannel.TrySubscribe(SetMenu))
                Debug.LogWarning($"{name}: No channel to set menu was provided");
        }

        private void OnDisable()
        {
            setMenuChannel.TryUnsubscribe(SetMenu);
        }

        private void SetMenu(Id menuId)
        {
            foreach (var menu in menus)
            {
                var isCurrentMenu = menu.ID == menuId;
                menu.gameObject.SetActive(isCurrentMenu);
                if (isCurrentMenu)
                    EventSystem.current.SetSelectedGameObject(menu.FirstSelection);
            }
        }
    }
}
