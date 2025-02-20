using BaseClasses;

namespace Implementations.PickUpItems
{
    public class ManaPotion : PickUp
    {
        public int restore;
        public override void Effect(CharacterSheet cs)
        {
            cs.RestoreMana(restore);
        }
    }
}