using BaseClasses;
using Implementations.Managers;

namespace Implementations.Teleporting
{
    public class LevelTwoCondition : TeleportPair
    {
        public override bool ConditionMet(Player player)
        {
            bool can = player.inventory.Contains("Orange Stone");
            if (!can)
            {
                EventDisplayManager.Instance.AddMessage("Acquire \"Orange Stone\" to unlock the next level");
            }
            return can;
        }
    }
}