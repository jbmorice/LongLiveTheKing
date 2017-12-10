using UnityEngine;

public class Siege : Agent
{
    public Village Village { get; set; }
    public Army Army { get; set; }

    public Siege(GameObject gameObject, Army army, Village village) :
        base(gameObject)
    {
        Army = army;
        Village = village;
    }

}
