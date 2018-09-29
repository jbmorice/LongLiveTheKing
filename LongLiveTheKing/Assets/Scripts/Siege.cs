namespace LLtK
{
    public class Siege : Agent
    {
        public Village Village { get; set; }
        public Army Army { get; set; }
        public bool InProgress = true;

        public void Init(GameManager gameManager, Army army, Village village)
        {
            GameManager = gameManager;
            Army = army;
            Village = village;
            GameManager.Sieges.Add(this);
            ResolveSiege resolveSiege = new ResolveSiege();
            resolveSiege.Start(this);
            Controller.AddAgentBehaviour(resolveSiege);
        }

        public void Remove()
        {
            GameManager.Sieges.Remove(this);
            Destroy(gameObject);
        }

        void Update()
        {
/*        if (!InProgress)
        {
            GameManager.Armies.Remove(Army);
            GameManager.Sieges.Remove(this);
            Destroy(Army.gameObject);
            Destroy(gameObject);
        }*/
        }
    }
}
