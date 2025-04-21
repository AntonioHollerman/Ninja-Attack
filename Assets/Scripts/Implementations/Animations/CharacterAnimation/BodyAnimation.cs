using UnityEngine;

namespace Implementations.Animations.CharacterAnimation
{
    public class BodyAnimation : MonoBehaviour
    {
        public SpriteRenderer body;

        public string hurtPath;
        public string spellCastPath;
        public string slashPath;
        public string walkPath;

        public Sprite[] hurtFrames;

        public Sprite[] spellCastLeftFrames;
        public Sprite[] spellCastRightFrames;
        public Sprite[] spellCastUpFrames;
        public Sprite[] spellCastDownFrames;
        
        void LateUpdate()
        {
            transform.rotation = Quaternion.identity; // Resets rotation to world space
        }
    }
}