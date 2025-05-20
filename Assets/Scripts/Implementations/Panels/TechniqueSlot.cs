using System;
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
        public SpriteRenderer sr;
        public int techniqueSlot;

        private Player _player;

        private void Awake()
        {
            _player = GameObject.Find("SoloPlayer")
                .transform
                .GetChild(0)
                .GetComponent<Player>();

            if (techniqueSlot == 1)
            {
                sr.sprite = TechniqueManager.GetSprites(_player.techOne.GetTechEnum())[0];
            }
            else
            {
                sr.sprite = TechniqueManager.GetSprites(_player.techOne.GetTechEnum())[1];
            }
        }

        public void Click()
        {
            if (master.workingSlot != null)
            {
                master.workingSlot.outlineImage.color = master.basicOutlineColor;
            }

            master.workingSlot = this;
            outlineImage.color = master.selectedOutlineColor;
        }
    }
}