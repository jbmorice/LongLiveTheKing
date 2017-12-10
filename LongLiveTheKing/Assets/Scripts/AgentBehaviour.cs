public abstract class AgentBehaviour
{
    protected AgentController _controller;
    protected bool _isRunning;
    protected bool _isPaused;


    public AgentController GetController()
    {
        return _controller;
    }

    public void SetController(AgentController controller)
    {
        _controller = controller;
    }

    public bool Start()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            return true;
        }
        return false;
        
    }

    public bool Stop()
    {
        if (_isRunning)
        {
            _isRunning = false;
            return true;
        }
        return false;
    }

    public bool Pause()
    {
        if (_isRunning)
        {
            _isPaused = true;
            return true;
        }
        return false;
    }

    public bool Resume()
    {
        if (_isRunning && _isPaused)
        {
            _isPaused = true;
            return true;
        }
        return false;
    }

    public abstract void Update(float dt);

}
