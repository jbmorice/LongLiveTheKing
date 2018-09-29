namespace LLtK
{
    public class ResolveSiege : AgentBehaviour
    {
        private Siege _siege;
        private float _period = 0.05f;
        private float _pastTime = 0.0f;
        private int _decrement = 1;

        public bool Start(Siege siege)
        {
            if (base.Start())
            {
                _siege = siege;
                foreach (PopulationProduction behaviour in _siege.Village.Controller.GetAgentBehaviours<PopulationProduction>())
                {
                    behaviour.Pause();
                }
                foreach (KingBoost behaviour in _siege.Village.Controller.GetAgentBehaviours<KingBoost>())
                {
                    behaviour.Pause();
                }

                _siege.Army.Controller.GetAgentBehaviour<GoTo>().Stop();
                return true;
            }
            return false;
        }

        public override void Update(float dt)
        {
            if (_siege.InProgress)
            {
                _pastTime += dt;
                if (_pastTime > _period)
                {
                    _siege.Army.Units -= _decrement;
                    _siege.Village.Population -= _decrement;
                    _pastTime -= _period;
                }

                if (_siege.Army.Kingdom == _siege.Village.Kingdom)
                {
                    _siege.Village.Population = _siege.Army.Units;
                    _siege.InProgress = false;
                    _siege.Army.Remove();
                    _siege.Remove();
                }
                if (_siege.Army.Units > 0 && _siege.Village.Population <= 0)
                {
                    _siege.Village.Population = _siege.Army.Units;

                    _siege.Village.Kingdom.RemovePossessedAgent(_siege.Village);
                    _siege.Army.Kingdom.AddPossessedAgent(_siege.Village);

                    _siege.Village.Kingdom = _siege.Army.Kingdom; // Move to AddPossessedAgent ?

                    _siege.InProgress = false;
                
                    _siege.Army.Remove();
                    _siege.Remove();
                
                    this.Stop();
                }
                else if (_siege.Army.Units <= 0)
                {
                    _siege.Army.Remove();
                    _siege.Remove();

                    _siege.InProgress = false;
                    this.Stop();
                }

            }

        }

        public bool Stop()
        {
            if (base.Stop())
            {
                foreach (PopulationProduction behaviour in _siege.Village.Controller.GetAgentBehaviours<PopulationProduction>())
                {
                    behaviour.Resume();
                }
                foreach (KingBoost behaviour in _siege.Village.Controller.GetAgentBehaviours<KingBoost>())
                {
                    behaviour.Resume();
                }
                return true;
            }
            return false;
        }
    }
}
