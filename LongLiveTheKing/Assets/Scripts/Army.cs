public class Army : MovingAgent
{
    private Kingdom _kingdom;
    private int _units;

    public Army(Kingdom kingdom, int units)
    {
        _kingdom = kingdom;
        _units = units;
    }
}
