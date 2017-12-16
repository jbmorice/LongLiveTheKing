using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadComponent : MonoBehaviour
{
    public Road Road { get; private set; }
    public VillageComponent FirstVillage;
    public VillageComponent SecondVillage;

    public void Init()
    {
        Road = new Road(this.gameObject, FirstVillage.Village, SecondVillage.Village);
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Vector3 position = (FirstVillage.transform.position + SecondVillage.transform.position) / 2;
        cube.transform.position = new Vector3(position[0],0.05f,position[2]);

        cube.transform.localScale = new Vector3(Vector3.Distance(FirstVillage.transform.position, SecondVillage.transform.position), 0.1f,0.5f);

        Vector3 vector = position - SecondVillage.transform.position;
        double angle;
        if (vector.normalized[2] < 0 && vector.normalized[0] < 0)
        {
            angle = (Math.Acos(vector.normalized[0]) * 180 / Math.PI);
            Debug.Log("if 1 " + angle);
        }
        else if (vector.normalized[2] < 0 && vector.normalized[0] > 0)
        {
            angle = (Math.Acos(vector.normalized[0]) * 180 / Math.PI);
            Debug.Log("if 2 " + angle);
        }
        else if (vector.normalized[2] > 0 && vector.normalized[0] < 0)
        {
            angle = -(Math.Acos(vector.normalized[0]) * 180 / Math.PI);
            Debug.Log("if 3 " + angle);
        }
        else
        {
            angle = -(Math.Acos(vector.normalized[0]) * 180 / Math.PI);
            Debug.Log("if 4" + angle);
        }
        Debug.Log(position);


        cube.transform.eulerAngles = new Vector3(0.0f,(float)angle,0.0f);
    }

    void Start ()
    {
        
    }

    void UpdateKingdom()
    {
        Kingdom kingdom = Road.BelongsToKingdom();
        if (kingdom != null)
        {
            this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = kingdom.UiColor;
        }
    }

    void Update () {
		UpdateKingdom();
	}
}
