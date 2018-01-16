using System.Collections.Generic;
using UnityEngine;

public class GoTo : AgentBehaviour
{
    private MovingAgent _agent;
    private List<Agent> _path;
    private int _position = 0;

    public Village Origin { get; private set; }
    public Agent Destination { get; private set; }


    public GoTo()
    {
        _path = new List<Agent>();
    }

    public bool Start(MovingAgent agent, Village origin, Agent destination)
    {
        if (base.Start())
        {
            _agent = agent;
            Destination = destination;
            _path.Add(destination);
            return true;
        }
        return false;
    }

    public override void Update(float dt)
    {
        float speed = 3;

        Vector3 start = _agent.gameObject.transform.position;
        Vector3 end = _path[_position].gameObject.transform.position;
        float distance = Vector3.Distance(start, end);
        float temp = dt * speed / distance;
        _agent.gameObject.transform.position =
            Vector3.Lerp(start, end, temp);
    }
}
