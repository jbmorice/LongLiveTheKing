namespace LLtK
{
    public class ResolveBattle : AgentBehaviour
    {
        private Battle _battle;
        private float _period = 0.02f;
        private float _pastTime = 0.0f;
        private int _decrement = 1;

        public bool Start(Battle battle)
        {
            if (base.Start())
            {
                _battle = battle;
                _battle.FirstAgent.Controller.GetAgentBehaviour<GoTo>().Pause();
                _battle.SecondAgent.Controller.GetAgentBehaviour<GoTo>().Pause();
                return true;
            }
            return false;
        }

        public bool Stop()
        {
            if (base.Stop())
            {
                _battle.IsActive = false;
                _battle.FirstAgent.Controller.GetAgentBehaviour<GoTo>().Resume();
                _battle.SecondAgent.Controller.GetAgentBehaviour<GoTo>().Resume();
                return true;
            }
            return false;
        }

        public override void Update(float dt)
        {
            _pastTime += dt;
            if (_pastTime > _period)
            {
                _battle.FirstAgent.Units -= _decrement;
                _battle.SecondAgent.Units -= _decrement;
                _pastTime -= _period;
            }

            if (_battle.FirstAgent.Units == 0)
            {
                this.Stop();
                _battle.InProgress = false;
                _battle.FirstHasFallen = true;
            }
            if (_battle.SecondAgent.Units == 0)
            {
                this.Stop();
                _battle.InProgress = false;
                _battle.SecondHasFallen = true;
            }
        }
    }
}
