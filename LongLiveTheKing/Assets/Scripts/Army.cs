using UnityEngine;

public class Army : MovingAgent
{
    public Army(GameObject gameObject, Kingdom kingdom, int units, Village origin, Agent destination) :
        base(gameObject)
    {
        Kingdom = kingdom;
        Units = units;

        // Add default behaviours
        GoTo goTo = new GoTo();;
        goTo.Start(this, origin, destination);
        Controller.AddAgentBehaviour(goTo);
    }

    public int Units { get; set; }
    public Kingdom Kingdom { get; set; }
}
