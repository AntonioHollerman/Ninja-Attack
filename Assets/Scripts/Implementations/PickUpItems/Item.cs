﻿using BaseClasses;
using Implementations.Managers;

namespace Implementations.PickUpItems
{
    public class Item : PickUp
    {
        public string itemName;
        public override void Effect(Player cs)
        {
            cs.inventory.Add(itemName);
            EventDisplayManager.Instance.AddMessage($"\"{itemName}\" has been added to inventory ");
        }
    }
}