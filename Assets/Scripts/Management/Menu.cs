using EventChannels.Runtime.Additions.Ids;
using UnityEngine;

namespace Management
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Id id;
        [SerializeField] private GameObject firstSelection;
        
        public Id ID => id;

        public GameObject FirstSelection => firstSelection;
    }
}