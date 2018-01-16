using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _clickHold = false;
    private Village _sourceVillage = null;
    private Village _destinationVillage = null;

    public List<Kingdom> Kingdoms;
    public List<Village> Villages;
    public List<Road> Roads;
    public List<Army> Armies;
    public List<Battle> Battles;
    public List<Siege> Sieges;
    public IA IA;

    public Kingdom Player;

    private void Init()
    {
        Object[] kingdoms = GameObject.FindObjectsOfType(typeof(Kingdom));

        foreach (Object obj in kingdoms)
        {
            Kingdom kingdom = (Kingdom) obj;
            kingdom.Init(this);
        }

        
        Object[] villages = GameObject.FindObjectsOfType(typeof(Village));

        foreach (Object obj in villages)
        {
            Village village = (Village) obj;
            village.Init(this, village.Kingdom);
        }

        Object[] roads = GameObject.FindObjectsOfType(typeof(Road));

        foreach (Object obj in roads)
        {
            Road road = (Road) obj;
            road.Init(this, road.FirstVillage, road.SecondVillage);
        }

        IA.Init(this);
        
    }

    void Start()
    {
        Kingdoms = new List<Kingdom>();
        Villages = new List<Village>();
        Roads = new List<Road>();
        Armies = new List<Army>();
        Sieges = new List<Siege>();
        Battles = new List<Battle>();

        Init();
    }

    void UpdateAgentBehaviours()
    {
        foreach (Kingdom kingdom in Kingdoms)
        {
            kingdom.Controller.Update(Time.deltaTime);
        }
        
        foreach (Village village in Villages)
        {
            village.Controller.Update(Time.deltaTime);
        }
        
        foreach (Road road in Roads)
        {
            road.Controller.Update(Time.deltaTime);
        }

        foreach (Army army in Armies)
        {
            army.Controller.Update(Time.deltaTime);
        }

        foreach (Siege siege in Sieges)
        {
            siege.Controller.Update(Time.deltaTime);
        }

        foreach (Battle battle in Battles)
        {
            battle.Controller.Update(Time.deltaTime);
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
                _sourceVillage = hit.transform.gameObject.GetComponent<Village>();
                if (_sourceVillage == null || !_sourceVillage.Kingdom.Equals(Player)) return;
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
                _destinationVillage = hit.transform.gameObject.GetComponent<Village>();
                if (_destinationVillage == null) return;
            }

            _sourceVillage.SendArmy(_destinationVillage);

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
