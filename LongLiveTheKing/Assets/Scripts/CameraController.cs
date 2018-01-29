using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class CameraController : MonoBehaviour
{

    public float PanSpeed = 20f;
    public float PanBorderThickness = 10f;
    public Vector3 PanLimit;

    public float ScrollSpeed = 20f;

    void Update ()
	{
	    Vector3 pos = transform.position;
	    if (Input.GetKey("z") || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - PanBorderThickness)
	    {
	        Debug.Log("Camera go up");
	        pos.z += PanSpeed * Time.deltaTime;
	    }
	    if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= PanBorderThickness)
	    {
	        Debug.Log("Camera go down");
	        pos.z -= PanSpeed * Time.deltaTime;
	    }
	    if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - PanBorderThickness)
	    {
	        Debug.Log("Camera go right");
	        pos.x += PanSpeed * Time.deltaTime;
	    }
	    if (Input.GetKey("q") || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= PanBorderThickness)
	    {
	        Debug.Log("Camera go left");
	        pos.x -= PanSpeed * Time.deltaTime;
	    }

        float scrool = Input.GetAxis("Mouse ScrollWheel");
	    pos.y -= ScrollSpeed * scrool * 1000f *  Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -PanLimit.x, PanLimit.x);
	    pos.z = Mathf.Clamp(pos.z, -PanLimit.z, PanLimit.z);
        pos.y = Mathf.Clamp(pos.y, 130, PanLimit.y);

        transform.position = pos;
	}
}
