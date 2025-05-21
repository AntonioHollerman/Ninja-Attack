using System;
using System.Collections;
using BaseClasses;
using Implementations.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Implementations.Panels
{
    public class TechniqueSlot : MonoBehaviour
    {
        public TechniquePanel master;
        public Image outlineImage;
        public Image iconImage;
        public int techniqueSlot;

        private Player _player;

        private void Awake()
        {
            _player = GameObject.Find("SoloPlayer")
                .transform
                .GetChild(0)
                .GetComponent<Player>();
        }

        public void LoadTech()
        {
            if (techniqueSlot == 1)
            {
                iconImage.sprite = TechniqueManager.GetSprites(_player.techOne.GetTechEnum())[0];
            }
            else
            {
                iconImage.sprite = TechniqueManager.GetSprites(_player.techTwo.GetTechEnum())[0];
            }
        }

        public void Click()
        {
            if (master.workingSlot == this)
            {
                master.workingSlot = null;
                outlineImage.color = master.basicOutlineColor;
            }
            
            if (master.workingSlot != null)
            {
                master.workingSlot.outlineImage.color = master.basicOutlineColor;
            }

            master.workingSlot = this;
            outlineImage.color = master.selectedOutlineColor;
        }
    }
}