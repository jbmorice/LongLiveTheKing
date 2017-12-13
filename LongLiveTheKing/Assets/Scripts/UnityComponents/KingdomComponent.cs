using UnityEngine;

public class KingdomComponent : MonoBehaviour
{
    public Kingdom Kingdom { get; private set; }
    public string Name;
    public Material Material;

    void Awake()
    {
        Kingdom = new Kingdom(gameObject, Name, Material.color);
    }

    void Start () {
        //this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Village.Kingdom.UiColor;
    }

    void Update () {
		
	}
}
