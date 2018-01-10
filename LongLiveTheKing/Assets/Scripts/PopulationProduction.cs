public class PopulationProduction : AgentBehaviour
{
    private Village _village;
    private float _period = 1.0f;
    private float _elapsedTime = 0.0f;
    private int _increment = 5;
    

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
        _elapsedTime += dt;
        if (_elapsedTime > _period)
        {
            _village.Population += _increment;
            _elapsedTime -= _period;
        }
    }
}
