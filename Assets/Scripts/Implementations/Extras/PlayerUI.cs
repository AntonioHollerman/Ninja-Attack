using System;
using BaseClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Implementations.Extras
{
    public class PlayerUI : MonoBehaviour
    {
        public bool UpdateFlagged;
        
        public Player player;
        
        public Image hpSlider;
        public Image manaSlider;
        public Image expSlider;

        
        public TextMeshProUGUI lvlText;
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI manaText;
        public TextMeshProUGUI expText;

        private void Update()
        {
            if (UpdateFlagged)
            {
                UpdateUi();
                UpdateFlagged = false;
            }
        }

        private void UpdateUi()
        {
            hpSlider.fillAmount = player.Hp / player.MaxHp;
            manaSlider.fillAmount = (float) player.Mana / player.MaxMana;
            expSlider.fillAmount = (float)player.Exp / player.ExpNeeded;

            lvlText.text = $"LvL\n{player.level}";
            hpText.text = $"{(int) player.Hp} / {(int) player.MaxHp}";
            manaText.text = $"{player.Mana} / {player.MaxMana}";
            expText.text = $"{player.Exp} / {player.ExpNeeded}";
        }
    }
}