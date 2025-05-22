using BaseClasses;

namespace Implementations.PickUpItems
{
    public class HpPotion : PickUp
    {
        public override void Effect(Player cs)
        {
            cs.RestoreHp(cs.MaxHp / 3);
        }
    }
}