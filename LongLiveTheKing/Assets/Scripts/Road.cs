using System;
using UnityEngine;

public class Road : Agent
{
    private float _travellingSpeed = 1.0f;
    public Village FirstVillage;
    public Village SecondVillage;

    public void Init(GameManager gameManager, Village firstVillage, Village secondVillage)
    {
        GameManager = gameManager;
        FirstVillage = firstVillage;
        SecondVillage = secondVillage;

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = transform;

        Vector3 position = (FirstVillage.transform.position + SecondVillage.transform.position) / 2;
        cube.transform.position = new Vector3(position[0], 0.05f, position[2]);

        cube.transform.localScale =
            new Vector3(Vector3.Distance(FirstVillage.transform.position, SecondVillage.transform.position), 0.1f,
                0.5f);

        Vector3 vector = position - SecondVillage.transform.position;
        double angle;
        if (vector.normalized[2] < 0 && vector.normalized[0] < 0)
        {
            angle = (Math.Acos(vector.normalized[0]) * 180 / Math.PI);
        }
        else if (vector.normalized[2] < 0 && vector.normalized[0] > 0)
        {
            angle = (Math.Acos(vector.normalized[0]) * 180 / Math.PI);
        }
        else if (vector.normalized[2] > 0 && vector.normalized[0] < 0)
        {
            angle = -(Math.Acos(vector.normalized[0]) * 180 / Math.PI);
        }
        else
        {
            angle = -(Math.Acos(vector.normalized[0]) * 180 / Math.PI);
        }

        cube.transform.eulerAngles = new Vector3(0.0f, (float)angle, 0.0f);

        FirstVillage.AddNeighbouringRoad(this);
        SecondVillage.AddNeighbouringRoad(this);

        GameManager.Roads.Add(this);

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

    void UpdateKingdom()
    {
        Kingdom kingdom = BelongsToKingdom();
        if (kingdom != null)
        {
            this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = kingdom.Material.color;
        }
    }

    void Update()
    {
        UpdateKingdom();
    }
}
