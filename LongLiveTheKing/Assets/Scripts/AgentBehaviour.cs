using UnityEngine;

public abstract class AgentBehaviour
{
    public enum State
    {
        Stopped,
        Running,
        Paused
    }

    protected AgentController _controller;
    protected State _state;

    public AgentController GetController()
    {
        return _controller;
    }

    public void SetController(AgentController controller)
    {
        _controller = controller;
    }

    public State Status
    {
        get
        {
            return _state;
        }

    }

    public bool Start()
    {
        if (_state == State.Stopped)
        {
            _state = State.Running;
            return true;
        }
        return false;
        
    }

    public bool Stop()
    {
        if (_state == State.Running)
        {
            _state = State.Stopped;
            return true;
        }
        return false;
    }

    public bool Pause()
    {
        if (_state == State.Running)
        {
            _state = State.Paused;
            return true;
        }
        return false;
    }

    public bool Resume()
    {
        if (_state == State.Paused)
        {
            _state = State.Running;
            return true;
        }
        return false;
    }

    public abstract void Update(float dt);

}
