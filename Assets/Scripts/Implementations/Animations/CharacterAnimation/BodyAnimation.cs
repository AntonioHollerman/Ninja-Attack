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

        public static float ForwardToDegrees(Vector3 f)
        {
            float x = Mathf.Clamp(f.x, -1, 1);
            float y = Mathf.Clamp(f.y, -1, 1);

            if (x != 0)
            {
                float result = Mathf.Atan(y / x) * Mathf.Rad2Deg;
                if (x < 0)
                {
                    result += 180f;
                }

                if (result < 0)
                {
                    result += 360;
                }

                return result;
            }
            else
            {
                float result = Mathf.Asin(y) * Mathf.Rad2Deg;
                return result;
            }
        }
        void LateUpdate()
        {
            transform.rotation = Quaternion.identity; // Resets rotation to world space
        }
    }
}