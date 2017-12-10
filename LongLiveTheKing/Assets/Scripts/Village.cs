using System.Collections.Generic;

public class Village : Agent
{
    private int _population = 0;
    private List<Road> _neighbouringRoads;
    private Kingdom _kingdom;

    public Village() : base()
    {

    }

    public int Population
    {
        get
        {
            return _population;
        }

        set
        {
            _population = value;
        }
    }

    public bool AddNeighbouringRoad(Road road)
    {
        bool contains = _neighbouringRoads.Contains(road);
        if (!contains)
        {
            _neighbouringRoads.Add(road);
            return true;
        }
        return false;
    }

    public bool RemoveNeighbouringRoad(Road road)
    {
        return _neighbouringRoads.Remove(road);
    }

    public Kingdom Kingdom
    {
        get
        {
            return _kingdom;
        }

        set
        {
            _kingdom = value;
        }
    }
}
