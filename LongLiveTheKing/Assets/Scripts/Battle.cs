using UnityEngine;

public class Battle : Agent
{
    public Army FirstAgent { get; set; }
    public Army SecondAgent { get; set; }
    public bool InProgress = true;
    public bool FirstHasFallen = false;
    public bool SecondHasFallen = false;

    public Battle(GameObject gameObject, Army firstAgent, Army secondAgent):
        base(gameObject)
    {
        FirstAgent = firstAgent;
        SecondAgent = secondAgent;
        ResolveBattle resolveBattle = new ResolveBattle();
        resolveBattle.Start(this);
        Controller.AddAgentBehaviour(resolveBattle);
    }

}
