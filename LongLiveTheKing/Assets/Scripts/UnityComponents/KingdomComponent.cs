using UnityEngine;

public class KingdomComponent : MonoBehaviour
{
    public Kingdom Kingdom { get; private set; }
    public string Name;
    public Material Material;

    public void Init()
    {
        Kingdom = new Kingdom(gameObject, Name, Material.color);
    }

    void Start () {
    }

    void Update () {
		
	}
}
