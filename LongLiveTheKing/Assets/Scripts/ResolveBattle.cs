public class ResolveBattle : AgentBehaviour
{
    private Battle _battle;
    private float _period = 0.2f;
    private float _pastTime = 0.0f;
    private int _decrement = 1;

    public bool Start(Battle battle)
    {
        if (base.Start())
        {
            _battle = battle;
            return true;
        }
        return false;
    }

    public bool Stop()
    {
        if (base.Stop())
        {
            _battle.IsActive = false;
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {
        for (_pastTime += dt; _pastTime < _period; _pastTime -= _period)
        {
            _battle.FirstAgent.Units -= _decrement;
            _battle.SecondAgent.Units -= _decrement;
        }

        if (_battle.FirstAgent.Units == 0)
        {
            this.Stop();
            _battle.FirstAgent.Kingdom.RemovePossessedAgent(_battle.FirstAgent);
        }
        if (_battle.SecondAgent.Units == 0)
        {
            this.Stop();
            _battle.SecondAgent.Kingdom.RemovePossessedAgent(_battle.SecondAgent);
        }
    }
}
