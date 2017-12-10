﻿using NUnit.Framework.Constraints;
using UnityEngine;

public class GoTo : AgentBehaviour
{
    private MovingAgent _agent;
    private Agent _destinastion;

    public bool Start(MovingAgent agent, Agent destination)
    {
        if (base.Start())
        {
            _agent = agent;
            _destinastion = destination;
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
