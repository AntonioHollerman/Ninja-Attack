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
            StartCoroutine(SelectListener());
        }

        public void BackButton()
        {
            PanelManager.Instance.SwapPanel(Panel.PauseGame);
        }

        public void ResumeButton()
        {
            PanelManager.Instance.CloseAllPanels();    
        }

        private IEnumerator SelectListener()
        {
            while (true)
            {
                yield return new WaitUntil(() => workingSlot != null && selectedTechnique != null);
                if (selectedTechnique.tech == _player.techOne.GetTechEnum() || selectedTechnique.tech == _player.techTwo.GetTechEnum())
                {
                    workingSlot.outlineImage.color = basicOutlineColor;
                    selectedTechnique.outlineImage.color = basicOutlineColor;
                    
                    workingSlot = null;
                    selectedTechnique = null;
                    continue;
                }
                
                workingSlot.sr.sprite = TechniqueManager.GetSprites(selectedTechnique.tech)[0];
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
                    
                    workingSlot = null;
                    selectedTechnique = null;
                    continue;
                }
                
            }
        }
    }
}