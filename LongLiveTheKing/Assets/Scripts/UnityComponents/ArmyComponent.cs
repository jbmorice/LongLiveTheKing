using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyComponent : MonoBehaviour {

    public Army Army {get; private set;}

    public void Init(KingdomComponent kingdomComponent, int units, VillageComponent origin, VillageComponent destination)
    {
        Army = new Army(gameObject, kingdomComponent.Kingdom, units, origin.Village, destination.Village);
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Army.Kingdom.UiColor;
    }
}
