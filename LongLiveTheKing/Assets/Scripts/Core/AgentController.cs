﻿using System.Collections.Generic;
using System.Linq;

namespace LLtK
{
    public class AgentController
    {
        private Agent _agent;
        private List<AgentBehaviour> _behaviours;

        public AgentController(Agent agent)
        {
            _agent = agent;
            _behaviours = new List<AgentBehaviour>();
        }

        public Agent Agent
        {
            get
            {
                return _agent;
            }
        }

        public void Update(float dt)
        {
            _behaviours.RemoveAll(behaviour => behaviour.Status == AgentBehaviour.State.Stopped);
            for (int i = 0; i < _behaviours.Count; i++)
            {
                if (_behaviours[i].Status == AgentBehaviour.State.Running)
                {
                    _behaviours[i].Update(dt);
                }
            }
        }

        public bool AddAgentBehaviour(AgentBehaviour behaviour)
        {
            bool contain = _behaviours.Contains(behaviour);
            if (!contain)
            {
                _behaviours.Add(behaviour);
                return true;
            }
            return false;

        }

        public bool RemoveAgentBehaviour(AgentBehaviour behaviour)
        {
            return _behaviours.Remove(behaviour);
        }

        public T GetAgentBehaviour<T>() where T : AgentBehaviour
        {
            foreach (AgentBehaviour agentBehaviour in _behaviours)
            {
                if (agentBehaviour.GetType() == typeof(T)) return (T) agentBehaviour;
            }

            return default(T);
        }

        public List<T> GetAgentBehaviours<T>() where T : AgentBehaviour
        {
            List<T> result = new List<T>();

            foreach (AgentBehaviour agentBehaviour in _behaviours)
            {
                if (agentBehaviour.GetType() == typeof(T)) result.Add((T) agentBehaviour);
            }

            return result;
        }
    }
}
