using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{
    private float _elapsedTimeAttack = 0.0f;
    private float _periodAttack = 15.0f;
    private float _elapsedTimeDefence = 5.0f;
    private float _periodDefence = 10.0f;


    public GameManager GameManager { get; private set; }

    public void Init(GameManager gameManager)
    {
        GameManager = gameManager;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Kingdom kingdom in GameManager.Kingdoms)
        {
            if (kingdom.IA)
            {
                if (!DefendVillages(kingdom, Time.deltaTime))
                {
                    Attack(kingdom, Time.deltaTime);
                }
            }
        }
    }

    List<Village> VillagesUnderSiege(Kingdom kingdom)
    {
        List<Village> villages = new List<Village>();

        foreach (Siege siege in GameManager.Sieges)
        {
            if (siege.Village.Kingdom == kingdom) villages.Add(siege.Village);
        }

        return villages;
    }

    bool DefendVillages(Kingdom kingdom, float dt)
    {
        List<Village> villagesUnderSiege = this.VillagesUnderSiege(kingdom);

        if (villagesUnderSiege.Count == 0)
        {
            return false;
        }
        else
        {
            _elapsedTimeDefence += dt;
            if (_elapsedTimeDefence > _periodDefence)
            {
                foreach (Village village in villagesUnderSiege)
                {
                    Village bestVillage = null;
                    bool first = true;

                    foreach (Road currentRoad in GameManager.Roads)
                    {
                        if (currentRoad.FirstVillage == village && currentRoad.SecondVillage.Kingdom == kingdom)
                        {
                            if (first)
                            {
                                bestVillage = currentRoad.SecondVillage;
                                first = false;
                            }
                            else
                            {
                                if (currentRoad.SecondVillage.Population > bestVillage.Population && !currentRoad.SecondVillage.IsUnderSiege())
                                    bestVillage = currentRoad.SecondVillage;
                            }
                        }
                        else if (currentRoad.SecondVillage == village && currentRoad.FirstVillage.Kingdom == kingdom)
                        {
                            if (first)
                            {
                                bestVillage = currentRoad.FirstVillage;
                                first = false;
                            }
                            else
                            {
                                if (currentRoad.FirstVillage.Population > bestVillage.Population && !currentRoad.FirstVillage.IsUnderSiege())
                                    bestVillage = currentRoad.FirstVillage;
                            }
                        }
                    }

                    if (bestVillage != null)
                    {
                        bestVillage.SendArmy(village);
                    }
                }

                _elapsedTimeDefence -= _periodDefence;
            }
        }
        return true;
    }

    List<Road> FindBordersRoads(Kingdom kingdom)
    {
        List<Road> roads = new List<Road>();

        foreach (Road road in GameManager.Roads)
        {
            if (road.FirstVillage.Kingdom == kingdom && road.SecondVillage.Kingdom != kingdom) roads.Add(road);
            else if (road.FirstVillage.Kingdom != kingdom && road.SecondVillage.Kingdom == kingdom) roads.Add(road);
        }

        return roads;
    }

    void LaunchAttack(Kingdom kingdom)
    {
        List<Road> bordersRoads = FindBordersRoads(kingdom);

        Village bestVillageToAttack = null;
        bool first = true;

        foreach (Road currentRoad in bordersRoads)
        {
            if (currentRoad.SecondVillage.Kingdom != kingdom)
            {
                if (first)
                {
                    bestVillageToAttack = currentRoad.SecondVillage;
                    first = false;
                }
                else
                {
                    if (currentRoad.SecondVillage.Population < bestVillageToAttack.Population)
                        bestVillageToAttack = currentRoad.SecondVillage;
                }
            }
            else if (currentRoad.FirstVillage.Kingdom != kingdom)
            {
                if (first)
                {
                    bestVillageToAttack = currentRoad.FirstVillage;
                    first = false;
                }
                else
                {
                    if (currentRoad.FirstVillage.Population < bestVillageToAttack.Population)
                        bestVillageToAttack = currentRoad.FirstVillage;
                }
            }
        }

        if (bestVillageToAttack != null)
        {
            first = true;
            Village bestVillage = null;

            foreach (Road currentRoad in bordersRoads)
            {
                if (currentRoad.FirstVillage == bestVillageToAttack)
                {
                    if (first)
                    {
                        bestVillage = currentRoad.SecondVillage;
                        first = false;
                    }
                    else
                    {
                        if (currentRoad.SecondVillage.Population > bestVillage.Population)
                            bestVillage = currentRoad.SecondVillage;
                    }
                }
                else if (currentRoad.SecondVillage == bestVillageToAttack)
                {
                    if (first)
                    {
                        bestVillage = currentRoad.FirstVillage;
                        first = false;
                    }
                    else
                    {
                        if (currentRoad.FirstVillage.Population > bestVillage.Population)
                            bestVillage = currentRoad.FirstVillage;
                    }
                }
            }

            if (bestVillage != null)
            {
                bestVillage.SendArmy(bestVillageToAttack);
            }
        }

    }

    void Attack(Kingdom kingdom, float dt)
    {
        _elapsedTimeAttack += dt;
        if (_elapsedTimeAttack > _periodAttack)
        {
            LaunchAttack(kingdom);
            _elapsedTimeAttack -= _periodAttack;
        }
    }
}
