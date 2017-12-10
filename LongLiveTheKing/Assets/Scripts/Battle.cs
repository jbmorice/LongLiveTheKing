public class Battle : Agent
{
    private Agent _firstAgent;
    private Agent _secondAgent;

    public Battle(Agent firstAgent, Agent secondAgent)
    {
        _firstAgent = firstAgent;
        _secondAgent = secondAgent;
    }

}
