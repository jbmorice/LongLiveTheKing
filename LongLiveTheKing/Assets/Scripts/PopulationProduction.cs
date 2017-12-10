using NUnit.Framework.Constraints;
using UnityEngine;

public class PopulationProduction : AgentBehaviour
{
    private Village _village;
    private float _period = 0.5f;
    private float _pastTime = 0.0f;
    private int _increment = 1;
    

    public bool Start(Village village)
    {
        if (base.Start())
        {
            _village = village;
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {   
        for (_pastTime += dt; _pastTime < _period; _pastTime -= _period)
        {
            _village.Population += _increment;
        }
    }
}
