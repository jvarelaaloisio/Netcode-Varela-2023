using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(fileName = "_" + nameof(CharacterSetup), menuName = "Models/Characters/Setup")]
    public class CharacterSetup : ScriptableObject
    {
        [field: SerializeField] public Sprite BaseSprite { get; set; }
        [field: SerializeField] public RuntimeAnimatorController AnimatorOverride { get; set; }
    }
}
