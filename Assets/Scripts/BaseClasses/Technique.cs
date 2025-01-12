using UnityEngine;

namespace BaseClasses
{
    /// <summary>
    /// will run following effects every frame
    /// <param name="cs">The combative character effects will be applied on</param>
    /// <param name="deltaTime">Used to help with applying logic per second</param>
    /// </summary>
    public delegate void StatusEffect(CharacterSheet cs, float deltaTime);
    
    public abstract class Technique : Weapon
    {
        public int ManaCost { get; protected set; } // How much implemented technique cost
        public float CoolDown { get; protected set; } // How many seconds is the cooldown
        
        /// <summary>
        /// UI of technique when equipped
        /// </summary>
        /// <returns>The path to the icon</returns>
        public abstract string GetIconPath();

        /// <summary>
        /// GameObject that will appear when ability is cast
        /// </summary>
        /// <returns>Path from Resources folder to the prefab</returns>
        public abstract string GetPrefabPath();
        

        // Compares via Prefab and icon
        public override bool Equals(object obj)
        {
            if (obj is Technique tech)
            {
                return GetPrefabPath() == tech.GetPrefabPath() && GetIconPath() == tech.GetIconPath();
            }
            return false;
        }

        // Hashcode is the addition Hashcode of GetPrefab() GetIconPath()
        public override int GetHashCode()
        {
            return GetPrefabPath().GetHashCode() + GetIconPath().GetHashCode();
        }
    }
}