using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Link to button on Main menu that opens a new pattern
    public void startNew() { 
        this.gameObject.SetActive(false);
        GameObject.Find("MainUI").GetComponent<UI>().activate();
        GameObject.Find("CentreButtonText").GetComponent<Text>().enabled = true;
    }

    //Loads up the loading interface, allowing the user to pick previously made patterns
    public void load()
    {
        //GameObject.Find("LoadUI").GetComponent<LoadUI>().activate();
    }

    //Opens the options page, for the user to edit program properties
    public void options()
    {
        this.gameObject.SetActive(false);
        //GameObject.Find("OptionUI").GetComponent<OptionUI>().activate();
    }



}
