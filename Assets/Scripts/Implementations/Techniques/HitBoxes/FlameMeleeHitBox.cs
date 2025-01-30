using BaseClasses;

namespace Implementations.Techniques.HitBoxes
{
    public class FlameMeleeHitBox : HitBox
    {
        public float baseDamage;
        public float burnEffectDps;
        public float duration;
        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(baseDamage);
            cs.LoadEffect(((sheet, deltaTime) => sheet.DealDamage(deltaTime * burnEffectDps)), duration);
        }
    }
}