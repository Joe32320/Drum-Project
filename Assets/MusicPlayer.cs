using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    
    float timeStart; // The time that playmode was entered, used to time the playing of sound files

    private List<DrumTrack> drumTracks; //List of all playing drum tracks
    float tempo; // the tempo, in beats per minute, defaults to 120
    private List<List<float>> beatTimings; // The time in a bar that a beat is to be played, 0 at the start of a bar, 1, at the end
    private List<List<bool>> beatsPlayed; // Whether the beat has been played or not
    private float previousTime; //Used to determine whether a new bar has been entered

    bool playMode;//Should music be playing?

    // Use this for initialization
    void Start()
    {
        tempo = 120f;
        tempo = 240f / tempo;

        playMode = false;

    }

    // Update is called once per frame, plays the beats from all drum tracks.
    void Update()
    {

        if (playMode)
        {
            float time = (Time.time - timeStart) % tempo;
            time = time / tempo;

            if (time < previousTime)
            {
                resetBeats();
            }


            for (int i = 0; i < drumTracks.Count; i++)
            {

                List<float> drumTimings = beatTimings[i];
                List<bool> drumBeatsPlayed = beatsPlayed[i];

                for (int j = 0; j < drumTimings.Count; j++)
                {

                    if (time > drumTimings[j] && !drumBeatsPlayed[j])
                    {
                        drumBeatsPlayed[j] = true;
                        drumTracks[i].playSound(j);
                        Debug.Log("Played");
                    }
                }
            }
            previousTime = time;
        }
    }

    //After a bar is played, sets a condition that all beats haven't been played, so are ready to play again in the coming bar
    private void resetBeats()
    {
        for (int i = 0; i < beatsPlayed.Count; i++)
        {
            for (int j = 0; j < beatsPlayed[i].Count; j++)
            {
                beatsPlayed[i][j] = false;
            }
        }
    }

    //Starts the music, method uses line angles in drumTracks to create the correct timings for each beat too
    public void playModeOn(List<GameObject> drums)
    {
        playMode = true;
        drumTracks = new List<DrumTrack>();
        for(int i = 0; i < drums.Count; i++)
        {
            drumTracks.Add(drums[i].GetComponent<DrumTrack>());
        }

        
        timeStart = Time.time;
        previousTime = timeStart;


        beatTimings = new List<List<float>>();
        beatsPlayed = new List<List<bool>>();


        for (int i = 0; i < drumTracks.Count; i++)
        {
            DrumTrack drum = drumTracks[i];

            List<float> drumTimings = new List<float>();
            List<bool> drumBeatsPlayed = new List<bool>();

            for (int j = 0; j < drum.getBeats(); j++)
            {
                float timing = drum.getLineAngle(j) / 360f;
                drumTimings.Add(timing);
                drumBeatsPlayed.Add(false);
            }
            drumTimings.Sort();
            beatTimings.Add(drumTimings);
            beatsPlayed.Add(drumBeatsPlayed);

        }


    }

    public void playModeOff()
    {
        playMode = false;
    }
}
