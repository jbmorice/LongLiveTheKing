using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public bool IsActive { get; set; }
    public GameManager GameManager { get; protected set; }
    public AgentController Controller { get; private set; }

    protected Agent()
    {
        Controller = new AgentController(this);
    }

}