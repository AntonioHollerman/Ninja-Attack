using System;
using BaseClasses;
using Implementations.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Implementations.Panels
{
    public class SelectableTechnique : MonoBehaviour
    {
        public TechniquePanel master;
        public Image outlineImage;

        public GameObject lockedGo;
        public SpriteRenderer sr;
        public Sprite active;
        public Sprite notActive;
        
        public int levelUnlock;
        public TechEnum tech;
        
        private Player _player;

        private void Awake()
        {
            _player = GameObject.Find("SoloPlayer")
                .transform
                .GetChild(0)
                .GetComponent<Player>();
            
            if (_player.level < levelUnlock || tech == TechEnum.IceShield)
            {
                lockedGo.SetActive(true);
                sr.sprite = notActive;
            }
            else
            {
                lockedGo.SetActive(false);
                sr.sprite = active;
            }
        }

        public void Click()
        {
            if (_player.level < levelUnlock)
            {
                EventDisplayManager.Instance.AddMessage($"To use '{tech}' you need to be level {levelUnlock}");
                return;
            }

            if (tech == TechEnum.IceShield)
            {
                EventDisplayManager.Instance.AddMessage("Ice Shield is not currently implemented");
                return;
            }
            if (master.selectedTechnique != null)
            {
                master.selectedTechnique.outlineImage.color = master.basicOutlineColor;
            }

            master.selectedTechnique = this;
            outlineImage.color = master.selectedOutlineColor;
        }
    }
}