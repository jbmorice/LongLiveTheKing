using System.Collections.Generic;
using UnityEngine;

public class Village : Agent
{
    public Kingdom Kingdom { get; set; }
    private int _population = 0;
    private List<Road> _neighbouringRoads;

    public Village(GameObject gameObject, Kingdom kingdom) :
        base(gameObject)
    {
        Kingdom = kingdom;
        Debug.Log("I am a village belonging to " + Kingdom.Name + "!");

        // Add default behaviours
        PopulationProduction populationProduction = new PopulationProduction();
        populationProduction.Start(this); // #FIXME : Override AgentBehaviour.Start() instead of overload
        Controller.AddAgentBehaviour(populationProduction);
    }

    public int Population
    {
        get
        {
            return _population;
        }

        set
        {
            _population = value;
        }
    }

    public bool AddNeighbouringRoad(Road road)
    {
        bool contains = _neighbouringRoads.Contains(road);
        if (!contains)
        {
            _neighbouringRoads.Add(road);
            return true;
        }
        return false;
    }

    public bool RemoveNeighbouringRoad(Road road)
    {
        return _neighbouringRoads.Remove(road);
    }

}
