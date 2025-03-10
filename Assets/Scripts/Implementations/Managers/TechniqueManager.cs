using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.Managers
{
    public class TechniqueManager : MonoBehaviour
    {
        public static TechniqueManager Instance { private set; get; }

        public GameObject activeTechniques;
        public Vector3 iconPlayerOneStartPos; // (-6, 3, 90.00)
        public Vector3 iconPlayerTwoStartPos; // (6, 3, 90.00)
        public float iconXDisplacement; // 1.463123 
        private void Awake()
        {
            if (this != null)
            {
                Destroy(gameObject);
                Destroy(activeTechniques);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public void LoadTechnique(TechEnum tech, CharacterSheet cs)
        {
            
        }

        public void LoadPlayerOneTechnique(TechEnum tech, Player player)
        {
            LoadTechnique(tech, player);
        }
        
        public void LoadPlayerTwoTechnique(TechEnum tech, Player player)
        {
            LoadTechnique(tech, player);
        }

        public static string GetTechniquePath(TechEnum tech)
        {
            return tech switch
            {
                TechEnum.FireBall => "prefabs/Techniques/FireBall.prefab",
                TechEnum.FireSword => "prefabs/Techniques/FireSword.prefab",
                TechEnum.StaticDischarge => "prefabs/Techniques/StaticDischarge.prefab",
                TechEnum.ElectricDash => "prefabs/Techniques/FlashStep.prefab",
                _ => "null"
            };
        }

        public static string GetIconPrefabPath(TechEnum tech)
        {
            return tech switch
            {
                TechEnum.FireBall => "prefabs/PlayerUI/TechniquesIcon/FireBall.prefab",
                TechEnum.FireSword => "prefabs/PlayerUI/TechniquesIcon/FireSword.prefab",
                TechEnum.StaticDischarge => "prefabs/PlayerUI/TechniquesIcon/Static Discharge.prefab",
                TechEnum.ElectricDash => "prefabs/PlayerUI/TechniquesIcon/Flash Step.prefab",
                _ => "null"
            };
        }
    }
}