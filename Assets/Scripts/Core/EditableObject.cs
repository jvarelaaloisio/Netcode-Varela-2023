using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EditableObject : MonoBehaviour
    {
        [SerializeField] private int maxHP;
        public int CurrentHp { get; private set; }
    }
}
