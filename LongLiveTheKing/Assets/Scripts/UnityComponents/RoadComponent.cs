using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadComponent : MonoBehaviour
{
    public Road Road { get; private set; }
    public VillageComponent FirstVillage { get; private set; }
    public VillageComponent SecondVillage { get; private set; }

    void Start () {
        Road = new Road(this.gameObject, FirstVillage.Village, SecondVillage.Village);		
	}

    void UpdateKingdom()
    {
        
    }
	
	void Update () {
		
	}
}
