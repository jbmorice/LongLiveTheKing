using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

// #TODO : Extract game related code to GameManager and only put camera movement here
public class CameraController : MonoBehaviour
{
    public GameObject GameManagerGameObject;
    private GameManager _gameMaager;

    public bool ClickHold = false;
    public VillageComponent sourceVillage = null;
    public VillageComponent destinationVillage = null;

    public GameObject ArmyPrefab;

    // Use this for initialization
    void Start ()
    {
        _gameMaager = GameManagerGameObject.GetComponent<GameManager>();
    }

    void MoveArmy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                sourceVillage = hit.transform.gameObject.GetComponent<VillageComponent>();
                if (sourceVillage == null || !sourceVillage.KingdomComponent.Kingdom.Equals(_gameMaager.Player.Kingdom)) return;
                Debug.Log("Source village : " + sourceVillage.name);
                ClickHold = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && sourceVillage != null && ClickHold)
        {
            ClickHold = false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                destinationVillage = hit.transform.gameObject.GetComponent<VillageComponent>();
                if (destinationVillage == null) return;
                Debug.Log("Destination village : " + destinationVillage.name);
            }

            if (sourceVillage == destinationVillage) return;

            SpawnArmy();

            // Reset attribute
            sourceVillage = null;
            destinationVillage = null;
        }

    }

    public void SpawnArmy()
    {
        GameObject army = Instantiate(ArmyPrefab, sourceVillage.transform);
        ArmyComponent armyComponent = army.GetComponent<ArmyComponent>();
        armyComponent.Init(_gameMaager.Player, 5, sourceVillage, destinationVillage);
        _gameMaager.AddArmyComponent(armyComponent);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    MoveArmy();
	}
}
