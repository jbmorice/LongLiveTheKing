using UnityEngine;

public class Siege : Agent
{
    public Village Village { get; set; }
    public Army Army { get; set; }
    public bool InProgress = true;

    public Siege(GameObject gameObject, Army army, Village village) :
        base(gameObject)
    {
        Army = army;
        Village = village;
        ResolveSiege resolveSiege = new ResolveSiege();
        resolveSiege.Start(this);
        Controller.AddAgentBehaviour(resolveSiege);
    }

}
