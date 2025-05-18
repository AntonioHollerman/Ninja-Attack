using BaseClasses;
using Implementations.Managers;
using UnityEngine;

namespace Implementations.Teleporting
{
    public class EnterBossRoomCondition : TeleportPair
    {
        public override bool ConditionMet(Player player)
        {
            bool can = player.inventory.Contains("Boss Key");

            if (!can)
            {
                EventDisplayManager.Instance.AddMessage("Find the \"Boss Key\" to proceed");
            }
            return can;
        }
    }
}