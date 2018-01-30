using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBoost : AgentBehaviour {

    private Village _village;
    private int maxPopulationIncrease = 100;
    private float _period = 1.0f;
    private float _elapsedTime = 0.0f;
    private int _increment = 5;


    public bool Start(Village village)
    {
        if (base.Start())
        {
            _village = village;
            _village.MaxPopulation += maxPopulationIncrease;
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {
        _elapsedTime += dt;
        if (_elapsedTime > _period)
        {
            _village.Population += _increment;
            _elapsedTime -= _period;
        }
    }

    public bool Stop()
    {
        if (base.Stop())
        {
            _village.MaxPopulation -= maxPopulationIncrease;
            return true;
        }
        return false;
    }
}
