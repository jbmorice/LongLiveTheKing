using UnityEngine;

public class VillageComponent : MonoBehaviour
{
    public KingdomComponent KingdomComponent;
    public GameManager GameManager { get; private set; }

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

    public void Init(GameManager gameManager)
    {
        GameManager = gameManager;
        Village = new Village(gameObject, KingdomComponent.Kingdom);
    }

    public ArmyComponent SendArmy(VillageComponent destinationVillage)
    {
        if (this == destinationVillage) return null;
        if (Village.Population < 2) return null;
        if (!Village.IsNeighbour(destinationVillage.Village)) return null;

        GameObject army = Instantiate(KingdomComponent.ArmyPrefab, transform);
        ArmyComponent armyComponent = army.GetComponent<ArmyComponent>();

        int oldPopulation = Population;
        int newPopulation = oldPopulation / 2;

        armyComponent.Init(GameManager, KingdomComponent, newPopulation, this, destinationVillage);
        Village.Population = oldPopulation - newPopulation;

        return armyComponent;
    }

    public bool IsUnderSiege()
    {
        foreach (SiegeComponent siege in GameManager.SiegesComponents)
        {
            if (Village == siege.VillageComponent.Village) return true;
        }
        return false;
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
