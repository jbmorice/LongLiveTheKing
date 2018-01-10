using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyComponent : MonoBehaviour {

    public Army Army {get; private set;}

    public int units;
    public int Units
    {
        get
        {
            return Army.Units;
        }
        set
        {
            units = value;
            Army.Units = value;
        }
    }

    public void Init(KingdomComponent kingdomComponent, int a_units, VillageComponent origin, VillageComponent destination)
    {
        Army = new Army(gameObject, kingdomComponent.Kingdom, a_units, origin.Village, destination.Village);
        Units = a_units;
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Army.Kingdom.UiColor;
    }
}
