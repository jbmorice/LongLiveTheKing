using UnityEngine;

public class VillageComponent : MonoBehaviour
{
    public KingdomComponent KingdomComponent;
    public Village Village { get; private set; }

	void Start () {
	    Village = new Village(gameObject, KingdomComponent.Kingdom);
	}

    void UpdateKingdom()
    {
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Village.Kingdom.UiColor;
    }

	void Update () {
		UpdateKingdom();
	}
}
