using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CentreButton : MonoBehaviour {

    Main main;
    Text text;
    

	// Use this for initialization
	void Start () {

        main = GameObject.Find("Main").GetComponent<Main>();
        text = GameObject.Find("CentreButtonText").GetComponent<Text>();
        text.enabled = false;
        

        if(main == null||text == null)
        {
            Debug.LogError("Failed to find object");

        }


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void show()
    {

    }

    public void pressed()
    {
        if (main.editModeActive())
        {
            text.text = "Play";
        }
        else
        {

        }



    }

    public void enteredEditMode()
    {
        text.text = "Reset";

    }

    public void exitedEditMode()
    {
        text.text = "Play";
    }


}
