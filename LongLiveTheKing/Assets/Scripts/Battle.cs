namespace LLtK
{
    public class Battle : Agent
    {
        public Army FirstAgent { get; set; }
        public Army SecondAgent { get; set; }
        public bool InProgress = true;
        public bool FirstHasFallen = false;
        public bool SecondHasFallen = false;

        public void Init(GameManager gameManager, Army firstAgent, Army secondAgent)
        {
            GameManager = gameManager;
            FirstAgent = firstAgent;
            SecondAgent = secondAgent;
            GameManager.Battles.Add(this);
            ResolveBattle resolveBattle = new ResolveBattle();
            resolveBattle.Start(this);
            Controller.AddAgentBehaviour(resolveBattle);
        }

        void Update()
        {
            if (!InProgress)
            {
                if (FirstHasFallen)
                {
                    FirstAgent.Remove();
                    Remove();
                }
                if (SecondHasFallen)
                {
                    SecondAgent.Remove();
                    Remove();
                }
            }
        }

    }
}
