public class Resolve : AgentBehaviour
{
    private Battle _battle;

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
            return true;
        }
        return false;
    }

    public bool Pause()
    {
        if (base.Pause())
        {
            return true;
        }
        return false;
    }

    public bool Resume()
    {
        if (base.Resume())
        {
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {

    }
}
