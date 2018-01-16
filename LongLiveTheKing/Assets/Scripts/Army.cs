﻿using UnityEngine;

public class Army : MovingAgent
{
    public int Units;
    public Kingdom Kingdom;
    public GameObject SiegePrefab;
    public GameObject BattlePrefab;

    public void Init(GameManager gameManager, Kingdom kingdom, int units, Village origin, Agent destination)
    {
        GameManager = gameManager;
        Kingdom = kingdom;
        Units = units;
        GameManager.Armies.Add(this);

        // Add default behaviours
        GoTo goTo = new GoTo();;
        goTo.Start(this, origin, destination);
        Controller.AddAgentBehaviour(goTo);
    }

    void OnTriggerEnter(Collider other)
    {
        Village village = other.transform.GetComponent<Village>();
        if (village && village == Controller.GetAgentBehaviour<GoTo>().Destination)
        {
            Village collidedVillage = other.gameObject.GetComponent<Village>();

            if (collidedVillage.Kingdom == Kingdom)
            {
                Debug.Log("J'ai rencontré un village allié !");

                collidedVillage.Population += Units;
                GameManager.Armies.Remove(this);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("J'ai rencontré un village ennemi !");
                //Controller.GetAgentBehaviour<GoTo>().Stop();
                GameObject obj = Instantiate(SiegePrefab, transform);
                obj.transform.position = (transform.position + collidedVillage.transform.position) / 2;
                Siege siege = obj.GetComponent<Siege>();
                siege.Init(GameManager, this, collidedVillage);
                GameManager.Sieges.Add(siege);
            }
        }

        Army collidedArmy = other.transform.GetComponent<Army>();
        if (collidedArmy)
        {
            if (collidedArmy.Kingdom == Kingdom)
            {
                Debug.Log("J'ai rencontré une armée allié !");
                bool iBesiege = Besiege();
                bool armyBesiege = collidedArmy.Besiege();
                if (iBesiege || armyBesiege)
                {
                    if (armyBesiege)
                    {
                        collidedArmy.Units += Units;
                        GameManager.Armies.Remove(this);
                        Destroy(gameObject);
                    }
                    else
                    {
                        Units += collidedArmy.Units;
                        GameManager.Armies.Remove(collidedArmy);
                        Destroy(collidedArmy.gameObject);
                    }
                }
            }
            else if (!InBattleAgainst(collidedArmy))
            {
                Debug.Log("J'ai rencontré une armée ennemi !");
                GameObject obj = Instantiate(BattlePrefab, transform);
                obj.transform.position = (transform.position + collidedArmy.transform.position) / 2;
                Battle battle = obj.GetComponent<Battle>();
                battle.Init(GameManager, this, collidedArmy);
                GameManager.Battles.Add(battle);
            }
        }
    }

    public bool Besiege()
    {
        foreach (Siege siege in GameManager.Sieges)
        {
            if (this == siege.Army) return true;
        }
        return false;
    }

    public bool InBattleAgainst(Army army)
    {
        foreach (Battle battle in GameManager.Battles)
        {
            if (battle.FirstAgent == this && battle.SecondAgent == army) return true;
            if (battle.FirstAgent == army && battle.SecondAgent == this) return true;
        }
        return false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Kingdom.Material.color;
    }
}
