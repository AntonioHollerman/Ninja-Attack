using System;
using System.Collections;
using BaseClasses;
using Implementations.Managers;
using UnityEngine;

namespace Implementations.Panels
{
    public class TechniquePanel : MonoBehaviour
    {
        public Color basicOutlineColor = new Color(130, 130, 130, 1);
        public Color selectedOutlineColor = Color.yellow;

        public TechniqueSlot workingSlot;
        public SelectableTechnique selectedTechnique;

        private Player _player;
        private void Awake()
        {
            _player = GameObject.Find("SoloPlayer")
                .transform
                .GetChild(0)
                .GetComponent<Player>();
            
            workingSlot = null;
            selectedTechnique = null;
        }

        private void Update()
        {
            if (PanelManager.Instance.activePanel != Panel.Techniques)
            {
                if (workingSlot != null)
                {
                    workingSlot.outlineImage.color = basicOutlineColor;
                }

                if (selectedTechnique != null)
                {
                    selectedTechnique.outlineImage.color = basicOutlineColor;
                }

                workingSlot = null;
                selectedTechnique = null;
            }
        }

        public void BackButton()
        {
            PanelManager.Instance.SwapPanel(Panel.PauseGame);
        }

        public void ResumeButton()
        {
            PanelManager.Instance.CloseAllPanels();    
        }

        private void UpdateIconLoc()
        {
            foreach (var key in _player.techDict.Keys)
            {
                _player.techDict[key].iconGo.SetActive(false);
            }
            
            _player.techOne.iconGo.SetActive(true);
            _player.techTwo.iconGo.SetActive(true);
            
            TechniqueManager.Instance.PlayerOneMoveTechniqueIconPosition(_player.techOne.iconGo, 0);
            TechniqueManager.Instance.PlayerOneMoveTechniqueIconPosition(_player.techTwo.iconGo, 1);
        }

        public void CheckForTechSwap()
        {
            if (selectedTechnique == null || workingSlot == null)
            {
                return;
            }
            Debug.Log("MADE IT!");
            if (selectedTechnique.tech == _player.techOne.GetTechEnum() || selectedTechnique.tech == _player.techTwo.GetTechEnum())
            {
                workingSlot.outlineImage.color = basicOutlineColor;
                selectedTechnique.outlineImage.color = basicOutlineColor;
                    
                workingSlot = null;
                selectedTechnique = null;
                return;
            }
            
            workingSlot.iconImage.sprite = TechniqueManager.GetSprites(selectedTechnique.tech)[0];
            if (_player.techDict.ContainsKey(selectedTechnique.tech))
            {
                if (workingSlot.techniqueSlot == 1)
                {
                    _player.techOne = _player.techDict[selectedTechnique.tech];
                }
                else
                {
                    _player.techTwo = _player.techDict[selectedTechnique.tech];
                }
            }
            else
            {
                if (workingSlot.techniqueSlot == 1)
                {
                    _player.techOne = TechniqueManager.Instance.LoadPlayerOneTechnique(
                        selectedTechnique.tech, 
                        _player, 
                        0);
                }
                else
                {
                    _player.techTwo = TechniqueManager.Instance.LoadPlayerOneTechnique(
                        selectedTechnique.tech, 
                        _player, 
                        1);
                }
            }
            
            workingSlot.outlineImage.color = basicOutlineColor;
            selectedTechnique.outlineImage.color = basicOutlineColor;
            workingSlot = null;
            selectedTechnique = null;
            UpdateIconLoc();
        }
    }
}