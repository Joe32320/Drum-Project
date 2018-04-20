using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public GameObject mainUIPanel;

    public Button addDrumButton;
    public Button removeDrumButton;
    public Button mainVolumeButton;
    public Button addBeatButton;
    public Button removeBeatButton;
    public Slider volumeSlider;
    public Button muteButton;
    public GameObject volumePanel;
    public Button mainMenuButton;

    public GameObject addDrumPanel;
    public GameObject removeDrumPanel;
    public GameObject addBeatPanel;
    public GameObject removeBeatPanel;

    public GameObject mainMenuReturnPanel;


    private float volumeLevel;

    private bool muted;


	// Use this for initialization
	void Start () {

        mainUIPanel.SetActive(false);
        volumePanel.SetActive(false);
        addDrumPanel.SetActive(false);
        //removeDrumPanel.SetActive(false);
        muted = false;
	}
	// Update is called once per frame
	void Update () {
		
	}

    //Allows the UI, along with its children UI elements to be seen and interacted with by the user
    public void activate()
    {
        mainUIPanel.SetActive(true);
    }

    //Hides the UI, along with child UI elements, and renders them unusable to the user
    public void deactivate()
    {
        mainUIPanel.SetActive(false);
    }

    //If AddDrumPanel is hidden, and unactive, activates it, otherwise hides the AddDrumPanel UI element and it's child components
    public void pressedAddDrumButton()
    {
        if (addDrumPanel.activeInHierarchy)
        {
            addDrumPanel.SetActive(false);
        }
        else
        {
            addDrumPanel.SetActive(true);
        }
    }

    //Activates or deactivates the removeDrumPanel if its already deactivated or activated respectfully
    public void pressedRemoveDrumButton()
    {

        if (removeDrumPanel.activeInHierarchy)
        {
            removeDrumPanel.SetActive(false);
        }
        else
        {
            removeDrumPanel.SetActive(true);
        }

    }

    //Activates or deactivates the volumePanel if its already deactivated or activated respectfully
    public void pressedMainVolumeButton()
    {
        if (volumePanel.activeInHierarchy)
        {
            volumePanel.SetActive(false);
        }
        else
        {
            volumePanel.SetActive(true);
        }


    }

    //Sets the overall volume level to 0, muting the sounds being played entirely if program is not already muted, else returns volume to level set on slider
    public void pressedMuteButton()
    {
        if (muted)
        {
            muted = false;
            GameObject.Find("Main").GetComponent<Main>().changeOverallVolume(volumeSlider.value);
        }
        else
        {
            muted = true;
            GameObject.Find("Main").GetComponent<Main>().changeOverallVolume(0f);
        }

    }

    //Sets the overall program volume, as long as program isn't already muted
    public void volumeLevelChanged()
    {
        if (!muted)
        {
            GameObject.Find("Main").GetComponent<Main>().changeOverallVolume(volumeSlider.value);
        }
    }

    public void pressedAddBeatButton()
    {

    }

    public void pressedRemoveBeatButton()
    {

    }

    public void pressedMainMenuButton()
    {

    }

    public void deactivatePanels()
    {

    }


}
