using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {


    public float size;//Size of the grpahic in worldspace units, is defaulted to 10
    public GameObject circle;//Prefab circle for creating beat circles
    public GameObject triangleMask; //triangle is 0.75 units high by default, is the mask responsible for givng the wedge shape
    public GameObject circleMask;//circle mask to hide the inner part of beat circles
    public GameObject line;// line, and tag prefab
    public float lineWidth; // The width of all lines dividing drum tracks and beats from one another
    public AudioClip[] soundFiles; //Array of drum sounds

    public float centreButtonSize; //Size in units of the centre button, defaulted to 1

    private List<GameObject> drumTracks;//List of all drum tracks

    private List<int> layerIDs; // List of Unity sorting layers

    private int numDrumTracks; //Number of drumtracks


    private bool isInEditMode; //Whether the program is in edit mode or not
    private GameObject editingDrumTrack; //Drum track that is being edited

    private float overallVolume; //Level of volume.


	// Use this for initialization
	void Start () {

        numDrumTracks = 0;
        setUpLayers();
        drumTracks = new List<GameObject>();
        centreButtonSize = 1f;
        overallVolume = 1f;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startNew()
    {

    }

    //Adds all user created layers in the editor for use for each individual drum track to have its own sorting layer
    void setUpLayers()
    {
        layerIDs = new List<int>();
        layerIDs.Add(SortingLayer.NameToID("1"));
        layerIDs.Add(SortingLayer.NameToID("2"));
        layerIDs.Add(SortingLayer.NameToID("3"));
        layerIDs.Add(SortingLayer.NameToID("4"));
        layerIDs.Add(SortingLayer.NameToID("5"));
        layerIDs.Add(SortingLayer.NameToID("6"));
        layerIDs.Add(SortingLayer.NameToID("7"));
        layerIDs.Add(SortingLayer.NameToID("8"));
        layerIDs.Add(SortingLayer.NameToID("9"));
        layerIDs.Add(SortingLayer.NameToID("10"));
        layerIDs.Add(SortingLayer.NameToID("11"));
        layerIDs.Add(SortingLayer.NameToID("12"));

    }

    //Adds and displays a newly created drumTrack
    public void addDrumTrack(int beats, int drumID)
    {


        numDrumTracks += 1;

        float quota = 1f / numDrumTracks;

        List<float> divisionList = new List<float>();
        for(int i = 0; i < numDrumTracks; i++)
        {
            divisionList.Add((i*quota*10f+1f));
            //Debug.Log(i * quota * 10f + 1f);
        }
        divisionList.Add(10f);
        for(int i = 0; i < divisionList.Count; i++)
        {
            //Debug.Log(divisionList[i]);
        }
        

        for(int i = 0; i < drumTracks.Count; i++)
        {

            drumTracks[i].GetComponent<DrumTrack>().resize(divisionList[i], divisionList[i+1],true);
            //Debug.Log(divisionList[i] + ":" + divisionList[i + 1]);
            
        }
        GameObject drumTrack = new GameObject();
        drumTrack.name = "DrumTrack";
        drumTrack.AddComponent<DrumTrack>();
        drumTrack.GetComponent<DrumTrack>().setUp(beats, triangleMask, circleMask, circle, layerIDs[10-numDrumTracks], new Color(Random.value, Random.value, Random.value), lineWidth, line, soundFiles[drumID]);
        drumTrack.GetComponent<DrumTrack>().resize(divisionList[divisionList.Count-2], 10,true);
        drumTracks.Add(drumTrack);
    }


    //Controls how mouse behaviour should be handled depending on what condition the program is in
    public void control(float mouseX, float mouseY, bool leftMouseButtonDown, bool leftMouseButtonHeld)
    {
        float r = Mathf.Sqrt((mouseX * mouseX) + (mouseY * mouseY)) * 2;
        float theta = findAngle(mouseX, mouseY);

        bool outOfBoundsClick;
        GameObject drum = getDrumTrackFromRadius(r, out outOfBoundsClick);

        if(r < 1) //This is the centre button area so this handles clicks on the centre button
        {
            if (isInEditMode&&leftMouseButtonDown) // If in edit mode, the centre button acts as the return point to the normal screen
            {
                exitEditMode();
            }
            else if (!isInEditMode&&leftMouseButtonDown) //Else, the centre button plays or stops playing the music
            {
                enterPlayMode();
            }
        }
        else
        {
            if (isInEditMode && leftMouseButtonHeld)
            {
                editingDrumTrack.GetComponent<DrumTrack>().editVolume(theta, r);
            }
            else if (!isInEditMode && leftMouseButtonDown)
            {
                enterEditMode(drum);
            }
        }
        
        

    }

    //Enters the program into edit mode, allowing a drum track to have its volume edited and beats moved
    public void enterEditMode(GameObject drumTrack)
    {
        for (int i = 0; i < drumTracks.Count; i++)
        {
            if (!drumTracks[i].Equals(drumTrack))
            {
                drumTracks[i].GetComponent<DrumTrack>().hide();
            }
        }

        isInEditMode = true;
        editingDrumTrack = drumTrack;
        drumTrack.GetComponent<DrumTrack>().enterEditMode();
        GameObject.Find("CentreButton").GetComponent<CentreButton>().enteredEditMode();
    }

    //Enters play mode and causes music to start
    public void enterPlayMode()
    {
        GameObject.Find("Background").GetComponent<MusicPlayer>().playModeOn(this.drumTracks);
        Debug.Log("Passed");
    }

    //Exit the edit mode, going back to displaying all the drum tracks
    public void exitEditMode()
    {
        for(int i = 0; i < drumTracks.Count; i++)
        {
            drumTracks[i].GetComponent<DrumTrack>().show();
            drumTracks[i].GetComponent<DrumTrack>().exitEditMode();

        }
        isInEditMode = false;
        GameObject.Find("CentreButton").GetComponent<CentreButton>().exitedEditMode();
        
    }

    public bool editModeActive()
    {
        return isInEditMode;
    }

    //Each drum track oppupies a ring, by supply a radius, r, the method returns the DrumTrack Gameobject covering this radius, and whether the click was out of bounds of the whole circle 
    public GameObject getDrumTrackFromRadius(float r, out bool outOfBounds)
    {
        for(int i = 0; i < drumTracks.Count; i++)
        {
            if (r < drumTracks[i].GetComponent<DrumTrack>().getMaximumRadius())
            {
                outOfBounds = false;
                return drumTracks[i];
            }
        }

        if(r < 1)
        {
            outOfBounds = true;
            return null;
        }
        if(r > 10)
        {
            outOfBounds = true;
            return null;
        }

        outOfBounds = true;
        Debug.LogError("Returning Unexpected Null value");
        return null;
    }

    //Returns the collider for use for detecting clicks on the graphic
    public BoxCollider2D getCollider()
    {
        return this.GetComponent<BoxCollider2D>();
    }

    public static float findAngle(float x, float y)
    {
        float mag = Mathf.Sqrt((x * x) + (y * y));
        float cosTheta = y / mag;
        float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
        if (x < 0)
        {
            theta = 360 - theta;
        }
        return theta;
    }

    public void changeOverallVolume(float newLevel)
    {
        if(newLevel < 0 || newLevel > 1)
        {
            Debug.Log("Error, value out of range");
        }
        overallVolume = newLevel;
    }

    public float getOverallVolume()
    {
        return overallVolume;
    }



}
