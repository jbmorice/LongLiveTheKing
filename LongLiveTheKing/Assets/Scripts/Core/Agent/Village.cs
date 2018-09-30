using System;
using System.Collections.Generic;
using UnityEngine;

namespace LLtK
{
    public class Village : Agent
    {
        [SerializeField]
        private Kingdom _kingdom;
        public Kingdom Kingdom
        {
            get
            {
                return _kingdom;
            }

            set
            {
                _kingdom = value;
                if (OnKingdomChange != null) OnKingdomChange(value);
            }
        }

        [SerializeField]
        private int _population = 10;
        public int Population
        {
            get
            {
                return _population;
            }

            set
            {
                _population = value;
                if (OnPopulationChange != null) OnPopulationChange(value);
            }
        }

        [SerializeField]
        private int _maxPopulation = 100;
        public int MaxPopulation
        {
            get
            {
                return _maxPopulation;
            }

            set
            {
                _maxPopulation = value;
                if (OnMaxPopulationChange != null) OnMaxPopulationChange(value);
            }
        }

        public bool IsPopulationIncreasing = true;
        public List<Road> NeighbouringRoads;

        public event Action<int> OnPopulationChange;
        public event Action<int> OnMaxPopulationChange;
        public event Action<Kingdom> OnKingdomChange;

        public void Init(GameManager gameManager, Kingdom kingdom)
        {
            GameManager = gameManager;
            Kingdom = kingdom;
            NeighbouringRoads = new List<Road>();
            GameManager.Villages.Add(this);
            Kingdom.AddPossessedAgent(this);
            Debug.Log("I am a village belonging to " + Kingdom.Name + "!");

            // Add default behaviours
            PopulationProduction populationProduction = new PopulationProduction();
            populationProduction.Start(this); // #FIXME : Override AgentBehaviour.Start() instead of overload
            Controller.AddAgentBehaviour(populationProduction);

            PopulationDiminution populationDiminution = new PopulationDiminution();
            populationDiminution.Start(this);
            populationDiminution.Pause();
            Controller.AddAgentBehaviour(populationDiminution);
        }

        public void InitUI()
        {
            // Update UI
            if (OnPopulationChange != null) OnPopulationChange(Population);
            if (OnMaxPopulationChange != null) OnMaxPopulationChange(MaxPopulation);
            if (OnKingdomChange != null) OnKingdomChange(Kingdom);
        }

        internal bool IsNeighbour(Village destinationVillage)
        {
            foreach (Road road in NeighbouringRoads)
            {
                if ((road.FirstVillage == this && road.SecondVillage == destinationVillage) ||
                    (road.FirstVillage == destinationVillage && road.SecondVillage == this))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Village> Neighbours()
        {
            List<Village> neighbours = new List<Village>();
            foreach (Road road in NeighbouringRoads)
            {
                if (road.FirstVillage == this)
                {
                    neighbours.Add(road.SecondVillage);
                }
                else
                {
                    neighbours.Add(road.FirstVillage);
                }
            }
            return neighbours;
        }

        public bool AddNeighbouringRoad(Road road)
        {
            bool contains = NeighbouringRoads.Contains(road);
            if (!contains)
            {
                NeighbouringRoads.Add(road);
                return true;
            }
            return false;
        }

        public bool RemoveNeighbouringRoad(Road road)
        {
            return NeighbouringRoads.Remove(road);
        }

        public Army SendArmy(Village destinationVillage)
        {
            if (this == destinationVillage) return null;
            if (Population < 2) return null;
            //if (!IsNeighbour(destinationVillage)) return null;

            List<Village> path = aStar(this, destinationVillage);

            Vector3 vector = path[1].transform.position - transform.position;
            vector = vector.normalized;

            double angle;
            if (vector[2] <= 0 && vector[0] <= 0)
            {
                angle = (Math.Acos(vector[0]) * 180 / Math.PI) + 180;
            }
            else if (vector[2] <= 0 && vector[0] > 0)
            {
                angle = (Math.Acos(vector[0]) * 180 / Math.PI) + 180;
            }
            else if (vector[2] > 0 && vector[0] <= 0)
            {
                angle = -(Math.Acos(vector[0]) * 180 / Math.PI) + 180;
            }
            else
            {
                angle = -(Math.Acos(vector[0]) * 180 / Math.PI) + 180;
            }

            GameObject obj = Instantiate(Kingdom.ArmyPrefab, transform);
            obj.transform.position = transform.position + (GetComponent<SphereCollider>().radius) * vector;
            obj.transform.eulerAngles = new Vector3(0.0f, (float)angle, 0.0f);

            Army army = obj.GetComponent<Army>();

            int oldPopulation = Population;
            int newPopulation = oldPopulation / 2;

            army.Init(GameManager, Kingdom, newPopulation, this, path );
            Population = oldPopulation - newPopulation;

            return army;
        }

        List<Village> aStar(Village start, Village goal)
        {
            List<Village> closedSet = new List<Village>();
            List<Village> openSet = new List<Village>();
            openSet.Add(start);

            Dictionary<Village, Village> cameFrom = new Dictionary<Village, Village>();
            Dictionary<Village, int> gScore = new Dictionary<Village, int>();
            Dictionary<Village, int> fScore = new Dictionary<Village, int>();

            foreach (Village village in GameManager.Villages)
            {
                gScore.Add(village, int.MaxValue);
                fScore.Add(village, int.MaxValue);
            }

            gScore[start] = 0;
            fScore[start] = heuristic_cost_estimate(start, goal);

            while (openSet.Count != 0)
            {
                Village current = null;
                int min = int.MaxValue;
                foreach (Village village in openSet)
                {
                    int temp = fScore[village];
                    if (min > temp)
                    {
                        current = village;
                        min = temp;
                    }
                }

                if (current == goal) return reconstruct_path(cameFrom, current);
                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Village neighbour in current.Neighbours())
                {
                    if (closedSet.Contains(neighbour)) continue;
                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    int tentativeGScore = gScore[current] + heuristic_cost_estimate(current, neighbour);
                    if (tentativeGScore >= gScore[neighbour]) continue;
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = gScore[neighbour] + heuristic_cost_estimate(neighbour, goal);
                }
            }
            return null;
        }

        int heuristic_cost_estimate(Village first, Village second)
        {
            int distance =
                (int) Vector3.Distance(first.gameObject.transform.position, second.gameObject.transform.position);
            if (second.Kingdom != Kingdom)
            {
                distance += 20;
            }
            return distance;
        }

        List<Village> reconstruct_path(Dictionary<Village, Village> cameFrom, Village current)
        {
            List<Village> path = new List<Village>();
            path.Add(current);
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse(0, path.Count);
            return path;
        }

        public bool IsUnderSiege()
        {
            foreach (Siege siege in GameManager.Sieges)
            {
                if (this == siege.Village) return true;
            }
            return false;
        }

        public bool IsUnderSiegeWith(Army army)
        {
            foreach (Siege siege in GameManager.Sieges)
            {
                if (this == siege.Village && army == siege.Army) return true;
            }
            return false;
        }

        void UpdateKingdom()
        {
            this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Kingdom.Material.color;
        }

        void Update()
        {
            UpdateKingdom();

            if (Population == MaxPopulation)
            {
                foreach (PopulationProduction populationProduction in Controller.GetAgentBehaviours<PopulationProduction>())
                {
                    populationProduction.Pause();
                }
                foreach (KingBoost kingBoost in Controller.GetAgentBehaviours<KingBoost>())
                {
                    kingBoost.Pause();
                }
                IsPopulationIncreasing = false;

                Controller.GetAgentBehaviour<PopulationDiminution>().Pause();
            }
            else if (Population > MaxPopulation)
            {
                Controller.GetAgentBehaviour<PopulationDiminution>().Resume();

                foreach (PopulationProduction populationProduction in Controller.GetAgentBehaviours<PopulationProduction>())
                {
                    populationProduction.Pause();
                }
                foreach (KingBoost kingBoost in Controller.GetAgentBehaviours<KingBoost>())
                {
                    kingBoost.Pause();
                }
                IsPopulationIncreasing = false;
            }
            else if(Population < MaxPopulation && !IsUnderSiege())
            {
                Controller.GetAgentBehaviour<PopulationDiminution>().Pause();

                foreach (PopulationProduction populationProduction in Controller.GetAgentBehaviours<PopulationProduction>())
                {
                    populationProduction.Resume();
                }
                foreach (KingBoost kingBoost in Controller.GetAgentBehaviours<KingBoost>())
                {
                    kingBoost.Resume();
                }
                IsPopulationIncreasing = true;
            }
        }
    }
}
