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
        public Image iconImage;
        private Sprite _active;
        private Sprite _notActive;
        
        public int levelUnlock;
        public TechEnum tech;
        
        private Player _player;

        private void OnEnable()
        {
            if (_player.level < levelUnlock || tech == TechEnum.IceShield)
            {
                lockedGo.SetActive(true);
                iconImage.sprite = _notActive;
            }
            else
            {
                lockedGo.SetActive(false);
                iconImage.sprite = _active;
            }
        }

        private void Awake()
        {
            _player = GameObject.Find("SoloPlayer")
                .transform
                .GetChild(0)
                .GetComponent<Player>();

            _active = TechniqueManager.GetSprites(tech)[0];
            _notActive = TechniqueManager.GetSprites(tech)[1];
            if (_player.level < levelUnlock || tech == TechEnum.IceShield)
            {
                lockedGo.SetActive(true);
                iconImage.sprite = _notActive;
            }
            else
            {
                lockedGo.SetActive(false);
                iconImage.sprite = _active;
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