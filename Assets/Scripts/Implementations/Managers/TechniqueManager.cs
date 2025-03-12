using System.Collections.Generic;
using BaseClasses;
using TMPro;
using UnityEngine;

namespace Implementations.Managers
{
    public class TechniqueManager : MonoBehaviour
    {
        public static TechniqueManager Instance { private set; get; }

        public GameObject statsUiCanvas;
        public GameObject techniquesIcons;
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
                Destroy(statsUiCanvas);
                Destroy(techniquesIcons);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(statsUiCanvas);
            DontDestroyOnLoad(techniquesIcons);
            DontDestroyOnLoad(activeTechniques);
        }
        public Technique LoadTechnique(TechEnum tech, CharacterSheet cs)
        {
            GameObject prefab = Resources.Load<GameObject>(GetTechniquePath(tech));
            GameObject techniqueGo = Instantiate(
                prefab,
                Vector3.zero,
                prefab.transform.rotation,
                activeTechniques.transform
                );

            Technique techScript = techniqueGo.GetComponent<Technique>();
            techScript.parent = cs;
            cs.techniques.Add(techScript);
            return techScript;
        }

        public Technique LoadPlayerOneTechnique(TechEnum tech, Player player, KeyCode code) 
        {
            GameObject iconPrefab = Resources.Load<GameObject>(GetIconPrefabPath(tech));
            GameObject iconGo = Instantiate(
                iconPrefab, 
                iconPlayerOneStartPos + new Vector3(iconXDisplacement * player.techniques.Count, 0, 0),
                iconPrefab.transform.rotation,
                techniquesIcons.transform);
            
            Transform keyBindTrans = iconGo.transform.Find("Keybind");
            Transform countDownTrans = iconGo.transform.Find("CountDown");
            keyBindTrans.gameObject.GetComponent<TextMeshProUGUI>().text = code.ToString();
            
            Technique techScript = LoadTechnique(tech, player);
            techScript.countDown = countDownTrans.gameObject.GetComponent<TextMeshProUGUI>();
            techScript.icon = iconGo.GetComponent<SpriteRenderer>();
            return techScript;
        }
        
        public Technique LoadPlayerTwoTechnique(TechEnum tech, Player player, KeyCode code) 
        {
            GameObject iconPrefab = Resources.Load<GameObject>(GetIconPrefabPath(tech));
            GameObject iconGo = Instantiate(
                iconPrefab, 
                iconPlayerTwoStartPos - new Vector3(iconXDisplacement * player.techniques.Count, 0, 0),
                iconPrefab.transform.rotation,
                techniquesIcons.transform);
            
            Transform keyBindTrans = iconGo.transform.Find("Keybind");
            Transform countDownTrans = iconGo.transform.Find("CountDown");
            keyBindTrans.gameObject.GetComponent<TextMeshProUGUI>().text = code.ToString();
            
            Technique techScript = LoadTechnique(tech, player);
            techScript.countDown = countDownTrans.gameObject.GetComponent<TextMeshProUGUI>();
            techScript.icon = iconGo.GetComponent<SpriteRenderer>();
            return techScript;
        }

        public static string GetTechniquePath(TechEnum tech)
        {
            return tech switch
            {
                TechEnum.FireBall        => "prefabs/Techniques/FireBall",
                TechEnum.FireSword       => "prefabs/Techniques/FireSword",
                TechEnum.StaticDischarge => "prefabs/Techniques/StaticDischarge",
                TechEnum.ElectricDash    => "prefabs/Techniques/FlashStep",
                _ => "null"
            };
        }

        public static string GetIconPrefabPath(TechEnum tech)
        {
            return tech switch
            {
                TechEnum.FireBall        => "prefabs/PlayerUI/TechniquesIcon/FireBall",
                TechEnum.FireSword       => "prefabs/PlayerUI/TechniquesIcon/FireSword",
                TechEnum.StaticDischarge => "prefabs/PlayerUI/TechniquesIcon/Static Discharge",
                TechEnum.ElectricDash    => "prefabs/PlayerUI/TechniquesIcon/Flash Step",
                _ => "null"
            };
        }
    }
}