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
        if (!IsNeighbour(destinationVillage)) return null;

        GameObject obj = Instantiate(Kingdom.ArmyPrefab, transform);
        Army army = obj.GetComponent<Army>();

        int oldPopulation = Population;
        int newPopulation = oldPopulation / 2;

        army.Init(GameManager, Kingdom, newPopulation, this, destinationVillage);
        Population = oldPopulation - newPopulation;

        return army;
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
