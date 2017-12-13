using System.Collections.Generic;
using UnityEngine;

public class Kingdom : Agent
{
    public string Name { get; private set; }
    private int _gold = 0;
    public Color UiColor { get; private set; }
    private List<Agent> _possessedAgents;

    public Kingdom(GameObject gameObject, string name, Color uiColor) : base(gameObject)
    {
        Name = name;
        UiColor = uiColor;
        Debug.Log("I am a kingdom named " + Name + " !");
    }

    public bool AddPossessedAgent(Agent agent)
    {
        bool contains = _possessedAgents.Contains(agent);
        if (!contains)
        {
            _possessedAgents.Add(agent);
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
        return _possessedAgents.Remove(agent);
    }
}
