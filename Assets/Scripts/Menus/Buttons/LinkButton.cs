using UnityEngine;
using UnityEngine.UI;

namespace Menus.Buttons
{
    [RequireComponent(typeof(Button))]
    public class LinkButton : MonoBehaviour
    {
        [SerializeField] private string url;
        private Button _button;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenLink);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenLink);
        }

        private void OpenLink()
        {
            Application.OpenURL(url);
        }
    }
}
