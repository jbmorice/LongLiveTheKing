using UnityEngine;

public class Battle : Agent
{
    public Army FirstAgent { get; set; }
    public Army SecondAgent { get; set; }

    public Battle(GameObject gameObject, Army firstAgent, Army secondAgent):
        base(gameObject)
    {
        FirstAgent = firstAgent;
        SecondAgent = secondAgent;
    }

}
