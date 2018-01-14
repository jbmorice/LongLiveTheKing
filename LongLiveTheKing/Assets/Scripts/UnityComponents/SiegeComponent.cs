using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeComponent : MonoBehaviour {
    public GameManager GameManager { get; private set; }
    public Siege Siege { get; private set; }
    public VillageComponent VillageComponent { get; private set; }
    public ArmyComponent ArmyComponent { get; private set; }

    public void Init(GameManager gameManager, ArmyComponent army, VillageComponent village)
    {
        GameManager = gameManager;
        Siege = new Siege(gameObject, army.Army, village.Village);
        VillageComponent = village;
        ArmyComponent = army;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (!Siege.InProgress)
	    {
            GameManager.ArmyComponents.Remove(ArmyComponent);
	        GameManager.SiegesComponents.Remove(this);
	        Destroy(ArmyComponent.gameObject);
            Destroy(gameObject);
        }
	}
}
