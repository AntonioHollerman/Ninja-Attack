using BaseClasses;
using Implementations.Managers;

namespace Implementations.Teleporting
{
    public class ExitBossRoomCondition : TeleportPair
    {
        public override bool ConditionMet(Player player)
        {
            bool can = player.defeatedBoss;
            if (!can)
            {
                EventDisplayManager.Instance.AddMessage("Defeat the boss before leaving");
            }
            return player.defeatedBoss;
        }
    }
}