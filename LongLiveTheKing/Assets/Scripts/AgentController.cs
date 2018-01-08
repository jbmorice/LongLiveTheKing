using System.Collections.Generic;

public class AgentController
{
    private Agent _agent;
    private List<AgentBehaviour> _behaviours;

    public AgentController(Agent agent)
    {
        _agent = agent;
        _behaviours = new List<AgentBehaviour>();
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
            if (behaviour.Status == AgentBehaviour.State.Running)
            {
                behaviour.Update(dt);
            }
            if (behaviour.Status == AgentBehaviour.State.Stopped)
            {
                RemoveAgentBehaviour(behaviour);
            }
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
