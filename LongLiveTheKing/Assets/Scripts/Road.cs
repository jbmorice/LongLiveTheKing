public class Road : Agent
{
    private float _travellingSpeed = 1.0f;
    private Village _firstVillage;
    private Village _secondVillage;

    public Road(Village firstVillage, Village secondVillage)
    {
        _firstVillage = firstVillage;
        _secondVillage = secondVillage;
    }

    public float TravellingSpeed
    {
        get
        {
            return _travellingSpeed;
        }

        set
        {
            _travellingSpeed = value;
        }
    }
}
