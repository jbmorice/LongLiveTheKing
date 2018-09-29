using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LLtK
{
    public class GameManager : MonoBehaviour
    {
        public bool GameInProgress = true;

        public GameObject VictoryUI;
        public GameObject DefeatUI;

        private bool _clickHoldArmy = false;
        private Village _sourceVillageArmy = null;
        private Village _destinationVillageArmy = null;

        private bool _clickHoldKing = false;
        private Village _sourceVillageKing = null;
        private Village _destinationVillageKing = null;

        public List<Kingdom> Kingdoms;
        public List<Village> Villages;
        public List<King> Kings;
        public List<Road> Roads;
        public List<Army> Armies;
        public List<Battle> Battles;
        public List<Siege> Sieges;
        public IA IA;

        public Kingdom Player;

        private void Init()
        {
            Object[] kingdoms = GameObject.FindObjectsOfType(typeof(Kingdom));

            foreach (Object obj in kingdoms)
            {
                Kingdom kingdom = (Kingdom)obj;
                kingdom.Init(this);
            }

            Object[] villages = GameObject.FindObjectsOfType(typeof(Village));

            foreach (Object obj in villages)
            {
                Village village = (Village)obj;
                village.Init(this, village.Kingdom);
            }

            foreach (Kingdom kingdom in Kingdoms)
            {
                if (kingdom.KingPrefab != null)
                {
                    kingdom.InitKing();
                }
            }

            Object[] roads = GameObject.FindObjectsOfType(typeof(Road));

            foreach (Object obj in roads)
            {
                Road road = (Road)obj;
                road.Init(this, road.FirstVillage, road.SecondVillage);
            }

            IA.Init(this);
        
        }

        void Start()
        {
            Kingdoms = new List<Kingdom>();
            Villages = new List<Village>();
            Kings = new List<King>();
            Roads = new List<Road>();
            Armies = new List<Army>();
            Sieges = new List<Siege>();
            Battles = new List<Battle>();

            Init();
        }

        private static bool IsAgentToRemove(Agent agent)
        {
            if (!agent.ToRemove) return false;
            agent.OnRemove();
            return true;
        }

        void UpdateAgentBehaviours()
        {
            foreach (Kingdom kingdom in Kingdoms)
            {
                kingdom.Controller.Update(Time.deltaTime);
            }
            Kingdoms.RemoveAll(IsAgentToRemove);

            foreach (King king in Kings)
            {
                king.Controller.Update(Time.deltaTime);
            }
            Kings.RemoveAll(IsAgentToRemove);

            foreach (Village village in Villages)
            {
                village.Controller.Update(Time.deltaTime);
            }
            Villages.RemoveAll(IsAgentToRemove);

            foreach (Road road in Roads)
            {
                road.Controller.Update(Time.deltaTime);
            }
            Roads.RemoveAll(IsAgentToRemove);
        
            foreach (Army army in Armies)
            {
                army.Controller.Update(Time.deltaTime);
            }
            Armies.RemoveAll(IsAgentToRemove);

            foreach (Siege siege in Sieges)
            {
                siege.Controller.Update(Time.deltaTime);
            }
            Sieges.RemoveAll(IsAgentToRemove);

            foreach (Battle battle in Battles)
            {
                battle.Controller.Update(Time.deltaTime);
            }
            Battles.RemoveAll(IsAgentToRemove);
        }

        void MoveArmy()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    _sourceVillageArmy = hit.transform.gameObject.GetComponent<Village>();
                    if (_sourceVillageArmy == null || !_sourceVillageArmy.Kingdom.Equals(Player) || _sourceVillageArmy.IsUnderSiege()) return;
                    _clickHoldArmy = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && _sourceVillageArmy != null && _clickHoldArmy)
            {
                _clickHoldArmy = false;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    _destinationVillageArmy = hit.transform.gameObject.GetComponent<Village>();
                    if (_destinationVillageArmy == null) return;
                }

                _sourceVillageArmy.SendArmy(_destinationVillageArmy);

                // Reset attributes
                _sourceVillageArmy = null;
                _destinationVillageArmy = null;
            }
        }

        void MoveKing()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    King king = hit.transform.gameObject.GetComponentInParent<King>();
                    if (king == null) return;

                    _sourceVillageKing = king.StayingVillage;

                    if (_sourceVillageKing == null || !_sourceVillageKing.Kingdom.Equals(Player)) return;
                    _clickHoldKing = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && _sourceVillageKing != null && _clickHoldKing)
            {
                _clickHoldKing = false;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    _destinationVillageKing = hit.transform.gameObject.GetComponent<Village>();
                    if (_destinationVillageKing == null || !_sourceVillageKing.Kingdom.Equals(Player) || _destinationVillageKing.Kingdom != Player) return;
                }

                King king = null;
                foreach (Agent agent in Player.PossessedAgents)
                {
                    if (agent.GetType() == typeof(King))
                    {
                        king = (King)agent;
                        break;
                    }
                }
                king.Move(_sourceVillageKing,_destinationVillageKing);

                // Reset attributes
                _sourceVillageKing = null;
                _destinationVillageKing = null;
            }

        }

        public void UpdateKing()
        {
            foreach (King king in Kings)
            {
                if (king.StayingVillage != null && king.Kingdom != king.StayingVillage.Kingdom)
                {
                    Debug.Log("I try to remove king " + king.Kingdom.Name);
                    king.Remove();
                }
            }
        }

        // FIXME: The victory should be triggered by what's causing it, not checked all the time
        public void CheckVictory()
        {
            King tempKing = null;
            foreach (King king in Kings)
            {
                if (king.Kingdom == Player) tempKing = king;
            }

            if (tempKing == null)
            {
                GameObject temp = Instantiate(DefeatUI, transform);
                temp.transform.SetParent(GameObject.Find("UICanvas").transform, true);
                GameInProgress = false;
                Debug.Log("Game Over");
            }
            else if (Kings.Count == 1)
            {
                GameObject temp = Instantiate(VictoryUI, transform);
                temp.transform.SetParent(GameObject.Find("UICanvas").transform, true);
                GameInProgress = false;
                Debug.Log("You win");
            }
        }

        void Update()
        {
            if (GameInProgress)
            {
                UpdateAgentBehaviours();
                MoveArmy();
                MoveKing();
                UpdateKing();
                CheckVictory();
            }
        
        }


    }
}
