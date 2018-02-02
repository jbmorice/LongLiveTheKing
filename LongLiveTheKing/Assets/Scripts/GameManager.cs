using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _clickHoldArmy = false;
    private Village _sourceVillageArmy = null;
    private Village _destinationVillageArmy = null;

    private bool _clickHoldKing = false;
    private Village _sourceVillageKing = null;
    private Village _destinationVillageKing = null;

    public List<Kingdom> Kingdoms;
    public List<Village> Villages;
    public List<King> Kings;
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

        foreach (Kingdom kingdom in Kingdoms)
        {
            if (kingdom.KingPrefab != null)
            {
                kingdom.InitKing();
            }
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
        Kings = new List<King>();
        Roads = new List<Road>();
        Armies = new List<Army>();
        Sieges = new List<Siege>();
        Battles = new List<Battle>();

        Init();
    }

    // #FIXME: Find a solution to modifying structures while iterating on it (separate list for agents to destroy after all updates)
    void UpdateAgentBehaviours()
    {
        foreach (Kingdom kingdom in Kingdoms)
        {
            kingdom.Controller.Update(Time.deltaTime);
        }

        foreach (King king in Kings)
        {
            king.Controller.Update(Time.deltaTime);
            //if(king.Kingdom == Player) Debug.Log(king.Controller.GetAgentBehaviours<GoTo>().Count);
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
                _sourceVillageArmy = hit.transform.gameObject.GetComponent<Village>();
                if (_sourceVillageArmy == null || !_sourceVillageArmy.Kingdom.Equals(Player) || _sourceVillageArmy.IsUnderSiege()) return;
                Debug.Log("Hey j'appuie sur un de mes villages");
                _clickHoldArmy = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && _sourceVillageArmy != null && _clickHoldArmy)
        {
            _clickHoldArmy = false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _destinationVillageArmy = hit.transform.gameObject.GetComponent<Village>();
                if (_destinationVillageArmy == null) return;
            }

            _sourceVillageArmy.SendArmy(_destinationVillageArmy);

            // Reset attributes
            _sourceVillageArmy = null;
            _destinationVillageArmy = null;
        }
    }

    void MoveKing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                King king = hit.transform.gameObject.GetComponentInParent<King>();
                if (king == null) return;

                _sourceVillageKing = king.StayingVillage;

                if (_sourceVillageKing == null || !_sourceVillageKing.Kingdom.Equals(Player)) return;
                Debug.Log("Hey j'appuie sur un de mes villages");
                _clickHoldKing = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && _sourceVillageKing != null && _clickHoldKing)
        {
            _clickHoldKing = false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _destinationVillageKing = hit.transform.gameObject.GetComponent<Village>();
                if (_destinationVillageKing == null || !_sourceVillageKing.Kingdom.Equals(Player) || _destinationVillageKing.Kingdom != Player) return;
            }

            King king = null;
            foreach (Agent agent in Player.PossessedAgents)
            {
                if (agent.GetType() == typeof(King))
                {
                    king = (King)agent;
                    break;
                }
            }
            king.Move(_sourceVillageKing,_destinationVillageKing);

            // Reset attributes
            _sourceVillageKing = null;
            _destinationVillageKing = null;
        }

    }

    public void UpdateKing()
    {
        foreach (King king in Kings)
        {
            if (king.StayingVillage != null && king.Kingdom != king.StayingVillage.Kingdom)
            {
                king.Remove();
            }
        }
    }

    public void CheckVictory()
    {
        King tempKing = null;
        foreach (King king in Kings)
        {
            if (king.Kingdom == Player) tempKing = king;
        }

        if (tempKing == null)
        {
            Debug.Log("Game Over");
        }

        if (Kings.Count == 1 && tempKing != null)
        {
            Debug.Log("You win");
        }
    }

    void Update()
    {
        CheckVictory();
        UpdateAgentBehaviours();
        MoveArmy();
        MoveKing();
        UpdateKing();
    }
}
