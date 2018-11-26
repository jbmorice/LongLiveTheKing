using System;
using System.Collections.Generic;
using UnityEngine;

namespace LLtK
{
    public class Kingdom : Agent
    {
        public string Name;
        public Material Material;
        public int Gold = 0;
        public List<Agent> PossessedAgents;
        public GameObject ArmyPrefab;
        public GameObject KingPrefab;
        public bool IA;

        public event Action<Kingdom> KingdomDestroyedEvent;

        public void Init(GameManager gameManager)
        {
            GameManager = gameManager;
            PossessedAgents = new List<Agent>();
            GameManager.Kingdoms.Add(this);
            Debug.Log("I am a kingdom named " + Name + " !");
        }

        public void InitKing()
        {
            GameObject obj = Instantiate(KingPrefab, transform);
            King king = obj.GetComponent<King>();
            king.Init(GameManager, this);
        }

        public bool AddPossessedAgent(Agent agent)
        {
            bool contains = PossessedAgents.Contains(agent);
            if (!contains)
            {
                PossessedAgents.Add(agent);
                if (agent.GetType() == typeof(Village))
                {
                    Village village = (Village) agent;
                    village.Kingdom = this;
                }
                return true;
            }
            return false;
        }

        public bool RemovePossessedAgent(Agent agent)
        {
            if (PossessedAgents.Count - 1 == 0)
            {
                Remove();
                if (KingdomDestroyedEvent != null) KingdomDestroyedEvent(this);
            }

            return PossessedAgents.Remove(agent);
        }
    }
}
