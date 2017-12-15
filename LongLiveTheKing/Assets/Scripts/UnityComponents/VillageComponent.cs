using UnityEngine;

public class VillageComponent : MonoBehaviour
{
    public KingdomComponent KingdomComponent;
    public Village Village { get; private set; }

    public void Init()
    {
        Village = new Village(gameObject, KingdomComponent.Kingdom);
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
	}
}
