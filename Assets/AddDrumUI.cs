using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AddDrumUI : MonoBehaviour {

    //Slider that dictates the number of beats to add to the new DrumTrack when created
    public Slider drumBeatSlider;
    //Text that displays number shown by the handle on the slider to let the user know what number of straight beats will be added when DrumTrack is created
    public Text sliderText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Called when the drumBeat slider is moved by the user. The value is held by the GameObject itself so isn't edited here, however the text needs to be changed to this new value hence this methods need
    public void sliderChanged()
    {
        sliderText.text = Mathf.RoundToInt(drumBeatSlider.value).ToString();
    }

    //Called when the user presses confirmed, will add the new drum track, and start the process of updating the graphic to correspond to the new state, also, if the main UI has not been activated 
    //for mouse clicks, this will activate it for the user to begin editing
    public void pressedConfirm()
    {
        GameObject.Find("Main").GetComponent<Main>().addDrumTrack(Mathf.RoundToInt(drumBeatSlider.value), 0);
        this.gameObject.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<Mouse>().activateCircleUI();

    }

    //Called when the user presses cancel, removes the AddDrumPanel from view
    public void pressedCancel()
    {
        this.gameObject.SetActive(false);
        //GameObject.Find("Main Camera").GetComponent<Mouse>().activateCircleUI();
    }



}
