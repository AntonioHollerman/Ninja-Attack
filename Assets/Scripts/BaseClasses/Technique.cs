using TMPro;
using UnityEngine;

namespace BaseClasses
{
    /// <summary>
    /// will run following effects every frame
    /// <param name="cs">The combative character effects will be applied on</param>
    /// <param name="deltaTime">Used to help with applying logic per second</param>
    /// </summary>
    public delegate void StatusEffect(CharacterSheet cs, float deltaTime);
    
    public abstract class Technique : MonoBehaviour
    {
        public CharacterSheet cs;
        public TextMeshProUGUI countDown;
        public SpriteRenderer icon;
        public Sprite active;
        public Sprite notActive;
        
        public int ManaCost { get; protected set; } // How much implemented technique cost
        public float CoolDown { get; protected set; } // How many seconds is the cooldown

        private Player _player;
    }
}