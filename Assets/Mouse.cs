using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour {

    Main main;
    CentreButton centreButton;
    bool isActive;

    
	// Use this for initialization
	void Start () {

        main = GameObject.Find("Main").GetComponent<Main>();
        centreButton = GameObject.Find("CentreButton").GetComponent<CentreButton>();

        if (main == null||centreButton == null)
        {
            Debug.LogError("Failed to find object");
        }
        isActive = false;


	}
	
	// Update is called once per frame
	void Update () {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        BoxCollider2D collider = main.getCollider();

   

        if (collider.OverlapPoint(mousePosition)&&isActive)
        {
            main.control(mousePosition.x, mousePosition.y, Input.GetMouseButtonDown(0), Input.GetMouseButton(0));
        }
	}

    public void activateCircleUI()
    {
        isActive = true;
    }

    public void deactivateCircleUI()
    {
        isActive = false;
    }

}
