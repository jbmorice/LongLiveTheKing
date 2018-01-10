using UnityEngine;

public class Road : Agent
{
    private float _travellingSpeed = 1.0f;
    public Village FirstVillage;
    public Village SecondVillage;

    public Road(GameObject gameObject, Village firstVillage, Village secondVillage) :
        base(gameObject)
    {
        FirstVillage = firstVillage;
        SecondVillage = secondVillage;
        Debug.Log("I am a road between " + FirstVillage + " and " + SecondVillage + " !");
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

    public Kingdom BelongsToKingdom()
    {
        if (FirstVillage.Kingdom == SecondVillage.Kingdom)
        {
            return FirstVillage.Kingdom;
        }
        return null;

    }
}
