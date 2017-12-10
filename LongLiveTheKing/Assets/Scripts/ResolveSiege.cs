public class ResolveSiege : AgentBehaviour
{
    private Siege _siege;
    private float _period = 0.2f;
    private float _pastTime = 0.0f;
    private int _decrement = 1;

    public bool Start(Siege siege)
    {
        if (base.Start())
        {
            _siege = siege;
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {
        for (_pastTime += dt; _pastTime < _period; _pastTime -= _period)
        {
            _siege.Army.Units -= _decrement;
            _siege.Village.Population -= _decrement;
        }

        if (_siege.Army.Units == 0)
        {
            this.Stop();
            _siege.Army.Kingdom.RemovePossessedAgent(_siege.Army);
        }
        if (_siege.Village.Population == 0)
        {
            this.Stop();
            _siege.Village.Kingdom.RemovePossessedAgent(_siege.Village);
        }
    }
}
