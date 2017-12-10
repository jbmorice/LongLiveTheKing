using System.Collections.Generic;

public class AgentController
{
    private Agent _agent;
    private List<AgentBehaviour> _behaviours;

    public AgentController(Agent agent)
    {
        _agent = agent;
    }

    public Agent Agent
    {
        get
        {
            return _agent;
        }
    }

    public void Update(float dt)
    {
        foreach (AgentBehaviour behaviour in _behaviours)
        {
            behaviour.Update(dt);
        }
    }

    public bool AddAgentBehaviour(AgentBehaviour behaviour)
    {
        bool contain = _behaviours.Contains(behaviour);
        if (!contain)
        {
            _behaviours.Add(behaviour);
            return true;
        }
        return false;

    }

    public bool RemoveAgentBehaviour(AgentBehaviour behaviour)
    {
            return _behaviours.Remove(behaviour);
    }
}
