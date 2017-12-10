using System.Collections.Generic;
using UnityEngine;

public class GoTo : AgentBehaviour
{
    private MovingAgent _agent;
    private Village _origin;
    private Agent _destinastion;
    private List<Agent> _path;
    private int _position = 0;

    public bool Start(MovingAgent agent, Village origin, Agent destination)
    {
        if (base.Start())
        {
            _agent = agent;
            _destinastion = destination;
            _path.Add(destination);
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {
        float speed = 1;

        Vector3 start = _agent.GameObject.transform.position;
        Vector3 end = _path[_position].GameObject.transform.position;
        float distance = Vector3.Distance(start, end);
        float temp = dt * speed / distance;
        _agent.GameObject.transform.position =
            Vector3.Lerp(start, end, temp);
    }
}
