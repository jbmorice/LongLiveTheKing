using UnityEngine;

public class Battle : Agent
{
    public Army FirstAgent { get; set; }
    public Army SecondAgent { get; set; }
    public bool InProgress = true;
    public bool FirstHasFallen = false;
    public bool SecondHasFallen = false;

    public void Init(GameManager gameManager, Army firstAgent, Army secondAgent)
    {
        GameManager = gameManager;
        FirstAgent = firstAgent;
        SecondAgent = secondAgent;
        GameManager.Battles.Add(this);
        ResolveBattle resolveBattle = new ResolveBattle();
        resolveBattle.Start(this);
        Controller.AddAgentBehaviour(resolveBattle);
    }

    void Update()
    {
        if (!InProgress)
        {
            if (FirstHasFallen)
            {
                GameManager.Armies.Remove(FirstAgent);
                GameManager.Battles.Remove(this);
                Destroy(FirstAgent.gameObject);
                Destroy(gameObject);
            }
            if (SecondHasFallen)
            {
                GameManager.Armies.Remove(SecondAgent);
                GameManager.Battles.Remove(this);
                Destroy(SecondAgent.gameObject);
                Destroy(gameObject);
            }
        }
    }

}
