using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyComponent : MonoBehaviour {

    public Army Army {get; private set;}
    public GameManager GameManager { get; private set; }

    public GameObject SiegePrefab;
    public GameObject BattlePrefab;

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

    public void Init(GameManager gameManager, KingdomComponent kingdomComponent, int a_units, VillageComponent origin, VillageComponent destination)
    {
        GameManager = gameManager;
        Army = new Army(gameObject, kingdomComponent.Kingdom, a_units, origin.Village, destination.Village);
        Units = a_units;
    }

    void OnTriggerEnter(Collider other)
    {
        VillageComponent villageComponent = other.transform.GetComponent<VillageComponent>();
        if (villageComponent && villageComponent.Village == Army.Controller.GetAgentBehaviour<GoTo>().Destination)
        {
            VillageComponent collidedVillage = other.gameObject.GetComponent<VillageComponent>();

            if (collidedVillage.Village.Kingdom == Army.Kingdom)
            {
                Debug.Log("J'ai rencontré un village allié !");

                collidedVillage.Village.Population += Army.Units;
                GameManager.ArmyComponents.Remove(this);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("J'ai rencontré un village ennemi !");
                Army.Controller.GetAgentBehaviour<GoTo>().Stop();
                GameObject siege = Instantiate(SiegePrefab, transform);
                siege.transform.position = (transform.position + collidedVillage.transform.position) / 2;
                SiegeComponent siegeComponent = siege.GetComponent<SiegeComponent>();
                siegeComponent.Init(GameManager, this, collidedVillage);
            }
        }
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Army.Kingdom.UiColor;
    }
}
