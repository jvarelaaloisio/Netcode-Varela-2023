using UnityEngine;

namespace Core.Characters
{
    public interface ICharacterSetup
    {
        /// <summary>
        /// Changes the base sprite on the sprite renderer
        /// </summary>
        /// <param name="newSprite"></param>
        void OverrideBaseSprite(Sprite newSprite);

        /// <summary>
        /// Changes the current Animator controller
        /// </summary>
        /// <param name="newController"></param>
        void OverrideAnimatorController(RuntimeAnimatorController newController);
    }
}
