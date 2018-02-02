using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageUI : MonoBehaviour
{
    public Village Village;

    public void Init(Village village)
    {
        Village = village;
        transform.SetParent(GameObject.Find("UICanvas").transform, true); // #TDOD: Check what the boolean actually does
        name = name + " - " + Village.name;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    Vector3 screenPosition = Camera.main.WorldToScreenPoint(Village.transform.position);
        screenPosition.z = 0.0f;
        screenPosition.y += 55;
        transform.position = screenPosition;

        GameObject backgroundPanel = transform.Find("Background").gameObject;
        if (backgroundPanel)
        {
            backgroundPanel.GetComponent<Image>().color = Village.Kingdom.Material.color;
        }
        transform.Find("Population").GetComponent<TextMeshProUGUI>().text = Village.Population.ToString();
        transform.Find("MaxPopulation").GetComponent<TextMeshProUGUI>().text = Village.MaxPopulation.ToString();
    }
}
