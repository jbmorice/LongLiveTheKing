using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<KingdomComponent> KingdomComponents;
    public List<VillageComponent> VillageComponents;
    public List<RoadComponent> RoadComponents;

    private void InitComponents()
    {
        Object[] kingdoms = GameObject.FindObjectsOfType(typeof(KingdomComponent));

        foreach (Object obj in kingdoms)
        {
            KingdomComponent kingdom = (KingdomComponent)obj;
            kingdom.Init();
            KingdomComponents.Add(kingdom);
        }

        Object[] villages = GameObject.FindObjectsOfType(typeof(VillageComponent));

        foreach (Object obj in villages)
        {
            VillageComponent village = (VillageComponent)obj;
            village.Init();
            VillageComponents.Add(village);
        }

        Object[] roads = GameObject.FindObjectsOfType(typeof(RoadComponent));

        foreach (Object obj in roads)
        {
            RoadComponent road = (RoadComponent)obj;
            road.Init();
            RoadComponents.Add(road);
        }
    }

    void Start()
    {
        KingdomComponents = new List<KingdomComponent>();
        VillageComponents = new List<VillageComponent>();
        RoadComponents = new List<RoadComponent>();

        InitComponents();
    }

    void UpdateAgentBehaviours()
    {
        foreach (KingdomComponent kingdomComponent in KingdomComponents)
        {
            kingdomComponent.Kingdom.Controller.Update(Time.deltaTime);
        }

        foreach (VillageComponent villageComponent in VillageComponents)
        {
            villageComponent.Village.Controller.Update(Time.deltaTime);
        }

        foreach (RoadComponent roadComponent in RoadComponents)
        {
            roadComponent.Road.Controller.Update(Time.deltaTime);
        }

    }

    void Update()
    {
        UpdateAgentBehaviours();
    }
}
