using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAComponent : MonoBehaviour
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
        foreach (KingdomComponent kingdomComponent in GameManager.KingdomComponents)
        {
            if (kingdomComponent.IA)
            {
                if (!DefendVillages(kingdomComponent, Time.deltaTime))
                {
                    Attack(kingdomComponent, Time.deltaTime);
                }
            }
        }
    }

    List<VillageComponent> VillagesUnderSiege(Kingdom kingdom)
    {
        List<VillageComponent> villages = new List<VillageComponent>();

        foreach (SiegeComponent siegeComponent in GameManager.SiegesComponents)
        {
            if (siegeComponent.Siege.Village.Kingdom == kingdom) villages.Add(siegeComponent.VillageComponent);
        }

        return villages;
    }

    bool DefendVillages(KingdomComponent kingdom, float dt)
    {
        List<VillageComponent> villagesUnderSiege = this.VillagesUnderSiege(kingdom.Kingdom);

        if (villagesUnderSiege.Count == 0)
        {
            return false;
        }
        else
        {
            _elapsedTimeDefence += dt;
            if (_elapsedTimeDefence > _periodDefence)
            {
                foreach (VillageComponent village in villagesUnderSiege)
                {
                    VillageComponent bestVillage = null;
                    bool first = true;

                    foreach (RoadComponent currentRoad in GameManager.RoadComponents)
                    {
                        if (currentRoad.FirstVillage == village && currentRoad.SecondVillage.KingdomComponent.Kingdom == kingdom.Kingdom)
                        {
                            if (first)
                            {
                                bestVillage = currentRoad.SecondVillage;
                                first = false;
                            }
                            else
                            {
                                if (currentRoad.SecondVillage.Population > bestVillage.Population && !currentRoad.SecondVillage.isUnderSiege())
                                    bestVillage = currentRoad.SecondVillage;
                            }
                        }
                        else if (currentRoad.SecondVillage == village && currentRoad.FirstVillage.KingdomComponent.Kingdom == kingdom.Kingdom)
                        {
                            if (first)
                            {
                                bestVillage = currentRoad.FirstVillage;
                                first = false;
                            }
                            else
                            {
                                if (currentRoad.FirstVillage.Population > bestVillage.Population && !currentRoad.FirstVillage.isUnderSiege())
                                    bestVillage = currentRoad.FirstVillage;
                            }
                        }
                    }

                    if (bestVillage != null)
                    {
                        ArmyComponent armyComponent = bestVillage.SendArmy(village);
                        if (armyComponent != null)
                        {
                            GameManager.AddArmyComponent(armyComponent);
                        }
                    }
                }

                _elapsedTimeDefence -= _periodDefence;
            }
        }
        return true;
    }

    List<RoadComponent> FindBordersRoads(Kingdom kingdom)
    {
        List<RoadComponent> roads = new List<RoadComponent>();

        foreach (RoadComponent road in GameManager.RoadComponents)
        {
            if (road.FirstVillage.KingdomComponent.Kingdom == kingdom && road.SecondVillage.KingdomComponent.Kingdom != kingdom) roads.Add(road);
            else if (road.FirstVillage.KingdomComponent.Kingdom != kingdom && road.SecondVillage.KingdomComponent.Kingdom == kingdom) roads.Add(road);
        }

        return roads;
    }

    void LaunchAttack(Kingdom kingdom)
    {
        List<RoadComponent> bordersRoads = FindBordersRoads(kingdom);

        VillageComponent bestVillageToAttack = null;
        bool first = true;

        foreach (RoadComponent currentRoad in bordersRoads)
        {
            if (currentRoad.SecondVillage.KingdomComponent.Kingdom != kingdom)
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
            else if (currentRoad.FirstVillage.KingdomComponent.Kingdom != kingdom)
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
            VillageComponent bestVillage = null;

            foreach (RoadComponent currentRoad in bordersRoads)
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
                ArmyComponent armyComponent = bestVillage.SendArmy(bestVillageToAttack);
                if (armyComponent != null)
                {
                    GameManager.AddArmyComponent(armyComponent);
                } 
            }
        }

    }

    void Attack(KingdomComponent kingdom, float dt)
    {
        _elapsedTimeAttack += dt;
        if (_elapsedTimeAttack > _periodAttack)
        {
            LaunchAttack(kingdom.Kingdom);
            _elapsedTimeAttack -= _periodAttack;
        }
    }
}
