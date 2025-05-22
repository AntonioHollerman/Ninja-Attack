using BaseClasses;

namespace Implementations.PickUpItems
{
    public class ManaPotion : PickUp
    {
        public int restore;
        public override void Effect(Player cs)
        {
            cs.RestoreMana(restore / 2);
        }
    }
}