using UnityEngine;

public class VillageComponent : MonoBehaviour
{
    public KingdomComponent KingdomComponent;
    public int population;
    public int Population
    {
        get { return Village.Population; }
        set
        {
            population = value;
            Village.Population = value;
        }
    }
    public Village Village { get; private set; }

    public void Init()
    {
        Village = new Village(gameObject, KingdomComponent.Kingdom);
    }

    public ArmyComponent SendArmy(VillageComponent destinationVillage)
    {
        if (Village.Population < 2) return null;

        GameObject army = Instantiate(KingdomComponent.ArmyPrefab, transform);
        ArmyComponent armyComponent = army.GetComponent<ArmyComponent>();

        int oldPopulation = Population;
        int newPopulation = oldPopulation / 2;

        armyComponent.Init(KingdomComponent, newPopulation, this, destinationVillage);
        Village.Population = oldPopulation - newPopulation;

        return armyComponent;
    }

    void Start ()
    {
        
    }

    void UpdateKingdom()
    {
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Village.Kingdom.UiColor;
    }

	void Update () {
		UpdateKingdom();
	    Population = Village.Population;
	}
}
