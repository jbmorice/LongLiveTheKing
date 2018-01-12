using UnityEngine;

public class KingdomComponent : MonoBehaviour
{
    public Kingdom Kingdom { get; private set; }
    public string Name;
    public Material Material;
    public GameManager GameManager { get; private set; }
    public GameObject ArmyPrefab;

    public void Init(GameManager gameManager)
    {
        GameManager = gameManager;
        Kingdom = new Kingdom(gameObject, Name, Material.color);
    }

    void Start () {
    }

    void Update () {
		
	}
}
