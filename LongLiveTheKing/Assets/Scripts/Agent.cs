using UnityEngine;

public abstract class Agent
{
    public bool IsActive { get; set; }
    public AgentController Controller { get; set; }
    public GameObject GameObject { get; set; }

    protected Agent(GameObject gameObject)
    {
        Controller = new AgentController(this);
        GameObject = gameObject;
    }

}