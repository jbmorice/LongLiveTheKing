using UnityEngine;

public class Army : MovingAgent
{
    public Army(GameObject gameObject, Kingdom kingdom, int units) :
        base(gameObject)
    {
        Kingdom = kingdom;
        Units = units;
    }

    public int Units { get; set; }
    public Kingdom Kingdom { get; set; }
}
