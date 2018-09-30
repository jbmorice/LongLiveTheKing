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

            village.OnPopulationChange += UpdatePopulation;
            village.OnMaxPopulationChange += UpdateMaxPopulation;
            village.OnKingdomChange += UpdateKingdom;

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

        private void UpdatePopulation(int population)
        {
            PopulationText.text = population.ToString();
        }

        private void UpdateMaxPopulation(int maxPopulation)
        {
            MaxPopulationText.text = maxPopulation.ToString();
        }

        private void UpdateKingdom(Kingdom kingdom)
        {
            BackgroundImage.color = kingdom.Material.color;
        }
    }
}
