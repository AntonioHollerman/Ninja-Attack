using BaseClasses;
using Implementations.Managers;

namespace Implementations.Teleporting
{
    public class LevelThreeCondition : TeleportPair
    {
        public override bool ConditionMet(Player player)
        {
            bool can = player.inventory.Contains("Grey Stone");
            
            if (!can)
            {
                EventDisplayManager.Instance.AddMessage("Acquire \"Grey Stone\" to unlock the next level");
            }
            return can;
        }
    }
}