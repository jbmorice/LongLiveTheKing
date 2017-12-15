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
