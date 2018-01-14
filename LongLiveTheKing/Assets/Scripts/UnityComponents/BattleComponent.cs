using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleComponent : MonoBehaviour {

    public GameManager GameManager { get; private set; }
    public Battle Battle { get; private set; }
    public ArmyComponent FirstArmyComponent { get; private set; }
    public ArmyComponent SecondArmyComponent { get; private set; }

    public void Init(GameManager gameManager, ArmyComponent first, ArmyComponent second)
    {
        GameManager = gameManager;
        Battle = new Battle(gameObject, first.Army, second.Army);
        FirstArmyComponent = first;
        SecondArmyComponent = second;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (!Battle.InProgress)
	    {
	        if (Battle.FirstHasFallen)
	        {
	            GameManager.ArmyComponents.Remove(FirstArmyComponent);
	            GameManager.BattleComponents.Remove(this);
	            Destroy(FirstArmyComponent.gameObject);
	            Destroy(gameObject);
            }
	        if(Battle.SecondHasFallen)
	        {
	            GameManager.ArmyComponents.Remove(SecondArmyComponent);
	            GameManager.BattleComponents.Remove(this);
	            Destroy(SecondArmyComponent.gameObject);
	            Destroy(gameObject);
            }
	    }
    }
}
