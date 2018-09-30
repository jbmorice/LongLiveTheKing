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
            gameManager.OnGameLost += GameLost;
            gameManager.OnGameWon += GameWon;

            foreach (Village village in gameManager.Villages)
            {
                VillageUI villageUI = Instantiate(VillageUIPrefab, transform).GetComponent<VillageUI>();
                villageUI.Init(village);
            }
        }

        private void GameLost(Kingdom kingdom)
        {
            Instantiate(DefeatUIPrefab, transform);
        }

        private void GameWon(Kingdom kingdom)
        {
            Instantiate(VictoryUIPrefab, transform);
        }
    }
}