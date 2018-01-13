using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeComponent : MonoBehaviour {
    public GameManager GameManager { get; private set; }
    public Siege Siege { get; private set; }

    public void Init(GameManager gameManager, ArmyComponent army, VillageComponent village)
    {
        GameManager = gameManager;
        Siege = new Siege(gameObject, army.Army, village.Village);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
