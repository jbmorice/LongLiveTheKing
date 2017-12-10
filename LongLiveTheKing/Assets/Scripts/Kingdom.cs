using System.Collections.Generic;
using UnityEngine;

public class Kingdom : Agent
{
    private string _name;
    private Color _uiColor;
    private int _gold = 0;
    private List<Agent> _possessedAgents;

    public Kingdom(string name, Color uiColor)
    {
        _name = name;
        _uiColor = uiColor;
    }

    public bool AddPossessedAgent(Agent agent)
    {
        bool contains = _possessedAgents.Contains(agent);
        if (!contains)
        {
            _possessedAgents.Add(agent);
            return true;
        }
        return false;
    }

    public bool RemoveNeighbouringRoad(Agent agent)
    {
        return _possessedAgents.Remove(agent);
    }
}
