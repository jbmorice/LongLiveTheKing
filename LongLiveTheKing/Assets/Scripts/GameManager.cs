using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    void Start()
    {
        Object[] kingdoms = GameObject.FindObjectsOfType(typeof(KingdomComponent));

        foreach (Object obj in kingdoms)
        {
            KingdomComponent kingdom = (KingdomComponent) obj;
            kingdom.Init();
        }

        Object[] villages = GameObject.FindObjectsOfType(typeof(VillageComponent));

        foreach (Object obj in villages)
        {
            VillageComponent village = (VillageComponent)obj;
            village.Init();
        }

        Object[] roads = GameObject.FindObjectsOfType(typeof(RoadComponent));

        foreach (Object obj in roads)
        {
            RoadComponent road = (RoadComponent)obj;
            road.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
