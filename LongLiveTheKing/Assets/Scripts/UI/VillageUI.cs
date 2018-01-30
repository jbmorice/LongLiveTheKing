﻿using System.Collections;
using System.Collections.Generic;
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
        transform.position = screenPosition;

	    transform.Find("VillagePopulation").GetComponent<Text>().text = Village.Population + "/" + Village.MaxPopulation;
    }
}