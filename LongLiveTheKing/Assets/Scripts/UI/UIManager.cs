using UnityEngine;

namespace LLtK.UI
{
    public class UIManager : MonoBehaviour
    {
        public GameObject VillageUIPrefab;
        public GameObject VictoryUIPrefab;
        public GameObject DefeatUIPrefab;

        public void Init(GameManager gameManager)
        {
            gameManager.GameLostEvent += OnGameLostEvent;
            gameManager.GameWonEvent += OnGameWonEvent;

            foreach (Village village in gameManager.Villages)
            {
                VillageUI villageUI = Instantiate(VillageUIPrefab, transform).GetComponent<VillageUI>();
                villageUI.Init(village);
            }
        }

        private void OnGameLostEvent(Kingdom kingdom)
        {
            Instantiate(DefeatUIPrefab, transform);
        }

        private void OnGameWonEvent(Kingdom kingdom)
        {
            Instantiate(VictoryUIPrefab, transform);
        }
    }
}