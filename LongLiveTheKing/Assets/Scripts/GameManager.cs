using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _clickHold = false;
    private VillageComponent _sourceVillage = null;
    private VillageComponent _destinationVillage = null;

    public List<KingdomComponent> KingdomComponents;
    public List<VillageComponent> VillageComponents;
    public List<RoadComponent> RoadComponents;
    public List<ArmyComponent> ArmyComponents;

    public KingdomComponent Player;

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

        ArmyComponents = new List<ArmyComponent>();
    }

    public void AddArmyComponent(ArmyComponent armyComponent)
    {
        ArmyComponents.Add(armyComponent);
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

        foreach (ArmyComponent armyComponent in ArmyComponents)
        {
            armyComponent.Army.Controller.Update(Time.deltaTime);
        }

    }

    void MoveArmy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _sourceVillage = hit.transform.gameObject.GetComponent<VillageComponent>();
                if (_sourceVillage == null || !_sourceVillage.KingdomComponent.Kingdom.Equals(Player.Kingdom)) return;
                Debug.Log("Source village : " + _sourceVillage.name);
                _clickHold = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && _sourceVillage != null && _clickHold)
        {
            _clickHold = false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _destinationVillage = hit.transform.gameObject.GetComponent<VillageComponent>();
                if (_destinationVillage == null) return;
                Debug.Log("Destination village : " + _destinationVillage.name);
            }

            if (_sourceVillage == _destinationVillage) return;

            ArmyComponent armyComponent = _sourceVillage.SendArmy(_destinationVillage);
            if (armyComponent != null)
            {
                AddArmyComponent(armyComponent);
            }

            // Reset attributes
            _sourceVillage = null;
            _destinationVillage = null;
        }

    }

    void Update()
    {
        UpdateAgentBehaviours();
        MoveArmy();
    }
}
