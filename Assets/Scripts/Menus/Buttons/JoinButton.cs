using Unity.Netcode;

namespace Menus
{
    public class JoinButton : ButtonBehaviour
    {
        protected override void HandleButtonClick()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
