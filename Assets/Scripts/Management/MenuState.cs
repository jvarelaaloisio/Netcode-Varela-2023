using System;
using System.Collections.Generic;
using System.Linq;
using EventChannels.Runtime.Additions.Ids;
using IA.FSM;
using UnityEngine;

namespace Management
{
    public class MenuState : MonoBehaviour, IState<Id>
    {
        [SerializeField] private List<Transition> transitions;
        
        public event Action OnAwake;
        public event Action OnSleep;

        public void HandleAwake()
            => gameObject.SetActive(true);

        public void HandleSleep()
            => gameObject.SetActive(false);

        public void AddTransition(Id key, IState<Id> transition) { }

        public bool TryGetTransition(Id key, out IState<Id> transition)
        {
            if (transitions.Any(t => t.key == key))
            {
                transition = transitions.First(t => t.key == key).destination;
                return true;
            }

            transition = null;
            return false;
        }

        [Serializable]
        public struct Transition
        {
            [SerializeField] public Id key;
            [SerializeField] public MenuState destination;
        }
    }
}