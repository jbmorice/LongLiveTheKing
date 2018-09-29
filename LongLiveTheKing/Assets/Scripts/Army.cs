using System;
using System.Collections.Generic;
using UnityEngine;

namespace LLtK
{
    public class Army : MovingAgent
    {
        public int Units;
        public Kingdom Kingdom;
        public GameObject SiegePrefab;
        public GameObject BattlePrefab;
        public List<Village> Path;
        public int CurrentDestination;

        public void Init(GameManager gameManager, Kingdom kingdom, int units, Village origin, List<Village> path)
        {
            GameManager = gameManager;
            Kingdom = kingdom;
            Units = units;
            Path = path;
            GameManager.Armies.Add(this);
            CurrentDestination = 1;

            // Add default behaviours
            GoTo goTo = new GoTo();;
            goTo.Start(this, origin, Path[1]);
            Controller.AddAgentBehaviour(goTo);
        }

        void OnTriggerEnter(Collider other)
        {
            Village village = other.transform.GetComponent<Village>();
            if (village && village == Path[Path.Count-1])
            {
                Village collidedVillage = other.gameObject.GetComponent<Village>();

                if (collidedVillage.Kingdom == Kingdom)
                {
                    //Debug.Log("J'ai rencontré un village allié !");

                    collidedVillage.Population += Units;
                    Remove();
                }
                else
                {
                    if (!collidedVillage.IsUnderSiegeWith(this))
                    {
                        //Debug.Log("J'ai rencontré un village ennemi !");
                        //Controller.GetAgentBehaviour<GoTo>().Stop();
                        GameObject obj = Instantiate(SiegePrefab, transform);
                        obj.transform.position = (transform.position + collidedVillage.transform.position) / 2;
                        Siege siege = obj.GetComponent<Siege>();
                        siege.Init(GameManager, this, collidedVillage);
                    }
                }
            }
            else if (village && village == Path[CurrentDestination])
            {
                Village collidedVillage = other.gameObject.GetComponent<Village>();

                if (collidedVillage.Kingdom == Kingdom)
                {
                    //Debug.Log("J'ai rencontré un village allié !");

                    CurrentDestination++;
                    Controller.GetAgentBehaviour<GoTo>().Stop();

                    Vector3 vector = Path[CurrentDestination].transform.position - Path[CurrentDestination - 1].transform.position;
                    vector = vector.normalized;

                    double angle;
                    if (vector[2] <= 0 && vector[0] <= 0)
                    {
                        angle = (Math.Acos(vector[0]) * 180 / Math.PI) + 180;
                    }
                    else if (vector[2] <= 0 && vector[0] > 0)
                    {
                        angle = (Math.Acos(vector[0]) * 180 / Math.PI) + 180;
                    }
                    else if (vector[2] > 0 && vector[0] <= 0)
                    {
                        angle = -(Math.Acos(vector[0]) * 180 / Math.PI) + 180;
                    }
                    else
                    {
                        angle = -(Math.Acos(vector[0]) * 180 / Math.PI) + 180;
                    }

                    transform.position = Path[CurrentDestination - 1].transform.position + Path[CurrentDestination - 1].GetComponent<SphereCollider>().radius * vector;
                    transform.eulerAngles = new Vector3(0.0f, (float)angle, 0.0f);

                    GoTo goTo = new GoTo();
                    goTo.Start(this, Path[CurrentDestination - 1], Path[CurrentDestination]);
                    Controller.AddAgentBehaviour(goTo);
                }
                else
                {
                    //Debug.Log("J'ai rencontré un village ennemi !");
                    //Controller.GetAgentBehaviour<GoTo>().Stop();
                    GameObject obj = Instantiate(SiegePrefab, transform);
                    obj.transform.position = (transform.position + collidedVillage.transform.position) / 2;
                    Siege siege = obj.GetComponent<Siege>();
                    siege.Init(GameManager, this, collidedVillage);
                }
            }

            Army collidedArmy = other.transform.GetComponent<Army>();
            if (collidedArmy)
            {
                if (collidedArmy.Kingdom == Kingdom)
                {
                    //Debug.Log("J'ai rencontré une armée allié !");
                    bool iBesiege = Besiege();
                    bool armyBesiege = collidedArmy.Besiege();
                    if (iBesiege || armyBesiege)
                    {
                        if (armyBesiege)
                        {
                            collidedArmy.Units += Units;
                            Remove();
                        }
                        else
                        {
                            Units += collidedArmy.Units;
                            collidedArmy.Remove();
                        }
                    }
                }
                else if (!InBattleAgainst(collidedArmy))
                {
                    //Debug.Log("J'ai rencontré une armée ennemi !");
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
    }
}
