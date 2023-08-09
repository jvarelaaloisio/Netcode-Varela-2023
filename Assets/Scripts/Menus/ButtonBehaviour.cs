using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private Button button;

        private void OnValidate()
        {
            button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(HandleButtonClick);
        }

        protected abstract void HandleButtonClick();
    }
}
