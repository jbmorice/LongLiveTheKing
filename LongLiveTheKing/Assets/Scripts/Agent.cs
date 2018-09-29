using UnityEngine;
using UnityEngine.Assertions;

namespace LLtK
{
    public abstract class Agent : MonoBehaviour
    {
        public bool IsActive { get; set; }
        public bool ToRemove { get; private set; }
        public GameManager GameManager { get; protected set; }
        public AgentController Controller { get; private set; }

        protected Agent()
        {
            Controller = new AgentController(this);
        }

        public void Remove()
        {
            Assert.IsFalse(ToRemove);
            ToRemove = true;
        }

        public virtual void OnRemove()
        {
            Destroy(gameObject);
        }
    }
}