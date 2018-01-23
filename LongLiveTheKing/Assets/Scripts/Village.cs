using System;
using System.Collections.Generic;
using UnityEngine;

public class Village : Agent
{
    public Kingdom Kingdom;
    public int Population = 10;
    public int MaxPopulation = 100;
    public bool IsPopulationIncreasing = true;
    public List<Road> NeighbouringRoads;

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

    List<Village> Neighbours()
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

        Vector3 vector = destinationVillage.transform.position - transform.position;
        vector = vector.normalized;

        GameObject obj = Instantiate(Kingdom.ArmyPrefab, transform);
        obj.transform.position = transform.position + 2 * vector;
        Army army = obj.GetComponent<Army>();

        int oldPopulation = Population;
        int newPopulation = oldPopulation / 2;

        army.Init(GameManager, Kingdom, newPopulation, this, aStar(this, destinationVillage));
        Population = oldPopulation - newPopulation;

        //army.Path = aStar(this, destinationVillage);

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

    void Start()
    {

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
            if (IsPopulationIncreasing)
            {
                foreach (PopulationProduction populationProduction in Controller.GetAgentBehaviours<PopulationProduction>())
                {
                    populationProduction.Pause();
                }
                IsPopulationIncreasing = false;
            }
            else
            {
                Controller.GetAgentBehaviour<PopulationDiminution>().Pause();
            }
        }
        else if (Population > MaxPopulation && IsPopulationIncreasing)
        {
            Controller.GetAgentBehaviour<PopulationDiminution>().Resume();

            foreach (PopulationProduction populationProduction in Controller.GetAgentBehaviours<PopulationProduction>())
            {
                populationProduction.Pause();
            }

            IsPopulationIncreasing = false;
        }
        else if(!IsPopulationIncreasing && Population < MaxPopulation)
        {
            Controller.GetAgentBehaviour<PopulationDiminution>().Pause();

            foreach (PopulationProduction populationProduction in Controller.GetAgentBehaviours<PopulationProduction>())
            {
                populationProduction.Resume();
            }
            IsPopulationIncreasing = true;
        }
    }

}
