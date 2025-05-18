using BaseClasses;

namespace Implementations.PickUpItems
{
    public class Item : PickUp
    {
        public string itemName;
        public override void Effect(Player cs)
        {
            cs.inventory.Add(itemName);
        }
    }
}