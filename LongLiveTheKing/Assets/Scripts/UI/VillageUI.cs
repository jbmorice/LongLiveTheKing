using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LLtK.UI
{
    public class VillageUI : MonoBehaviour
    {
        public Village Village;

        public Image BackgroundImage;
        public TextMeshProUGUI PopulationText;
        public TextMeshProUGUI MaxPopulationText;
        public float Elevation = 40.0f;

        public void Init(Village village)
        {
            Village = village;
            name = Village.name;

            village.PopulationChangeEvent += OnUpdatePopulation;
            village.MaxPopulationChangeEvent += OnUpdateMaxPopulation;
            village.KingdomChangeEvent += OnUpdateKingdom;

            village.InitUI();
        }
	
        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(Village.transform.position);
            screenPosition.z = 0.0f;
            screenPosition.y += Elevation;
            transform.position = screenPosition;
        }

        private void OnUpdatePopulation(int population)
        {
            PopulationText.text = population.ToString();
        }

        private void OnUpdateMaxPopulation(int maxPopulation)
        {
            MaxPopulationText.text = maxPopulation.ToString();
        }

        private void OnUpdateKingdom(Kingdom kingdom)
        {
            BackgroundImage.color = kingdom.Material.color;
        }
    }
}
