using BaseClasses;

namespace Implementations.PickUpItems
{
    public class HpPotion : PickUp
    {
        public float restore;
        public override void Effect(CharacterSheet cs)
        {
            cs.RestoreHp(restore);
        }
    }
}