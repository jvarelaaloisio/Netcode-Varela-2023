using UnityEngine;

namespace Core
{
    [CreateAssetMenu(menuName = "Models/Characters/Jump", fileName = "_JumpModel", order = 0)]
    public class JumpModel : ScriptableObject
    {
        [field: SerializeField] public int maxJumpQty { get; private set; } = 1;
    }
}
