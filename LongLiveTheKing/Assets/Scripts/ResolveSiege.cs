using UnityEngine;

public class ResolveSiege : AgentBehaviour
{
    private Siege _siege;
    private float _period = 0.1f;
    private float _pastTime = 0.0f;
    private int _decrement = 2;

    public bool Start(Siege siege)
    {
        if (base.Start())
        {
            _siege = siege;
            foreach (PopulationProduction behaviour in _siege.Village.Controller.GetAgentBehaviours<PopulationProduction>())
            {
                behaviour.Pause();
            }
            _siege.Army.Controller.GetAgentBehaviour<GoTo>().Stop();
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {
        Debug.Log("I update the siege !");


        _pastTime += dt;
        if (_pastTime > _period)
        { 
            _siege.Army.Units -= _decrement;
            _siege.Village.Population -= _decrement;
            _pastTime -= _period;
        }

        if (_siege.Army.Units > 0 && _siege.Village.Population <= 0)
        {
            this.Stop();

            _siege.Village.Population = _siege.Army.Units;
            _siege.Village.Kingdom = _siege.Army.Kingdom;

            _siege.Army.Kingdom.AddPossessedAgent(_siege.Village);
            _siege.Village.Kingdom.RemovePossessedAgent(_siege.Village);

            _siege.InProgress = false;
        }
        else if (_siege.Army.Units <= 0 )
        {
            this.Stop();
            _siege.InProgress = false;
        }

    }

    public bool Stop()
    {
        if (base.Stop())
        {
            foreach (PopulationProduction behaviour in _siege.Village.Controller.GetAgentBehaviours<PopulationProduction>())
            {
                behaviour.Resume();
            }
            return true;
        }
        return false;
    }
}
