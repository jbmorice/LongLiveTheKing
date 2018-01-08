using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class CameraController : MonoBehaviour
{
    public bool ClickHold = false;
    VillageComponent sourceVillage = null;
    VillageComponent destinationVillage = null;

    // Use this for initialization
    void Start () {
		
	}

    void MoveArmy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickHold = true;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                sourceVillage = hit.transform.gameObject.GetComponent<VillageComponent>();
                if (sourceVillage == null) return;
                Debug.Log("Source village : " + sourceVillage.name);
            }
        }
        if (Input.GetMouseButtonUp(0) && sourceVillage != null)
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

            // #TODO: Moving logic

            // Reset attribute
            sourceVillage = null;
            destinationVillage = null;
        }

    }
	
	// Update is called once per frame
	void Update ()
	{
	    MoveArmy();
	}
}
