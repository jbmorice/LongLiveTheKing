public abstract class Agent
{
    private AgentController _controller;

    protected Agent()
    {
        _controller = new AgentController(this);
    }

    public AgentController Controller
    {
        get
        {
            return _controller;
        }

        set
        {
           _controller = value;
        }
    }

    
}