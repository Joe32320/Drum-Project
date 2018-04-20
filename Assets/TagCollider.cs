using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagCollider : MonoBehaviour {

    BoxCollider2D collider;
    DrumTrack drumTrack;
    int beatNum;
    bool dragging;




	// Use this for initialization
	void Start () {
        collider = this.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (collider.OverlapPoint(mousePosition)&&this.GetComponent<SpriteRenderer>().enabled == true)
        {

            if (Input.GetMouseButtonDown(0)) //If mouse button pressed on tag, then activate dragging of beat
            {
                Debug.Log("Hitting" + beatNum);
                dragging = true;

            }
            
            
        }

        if (Input.GetMouseButtonUp(0) && dragging) // If mouse button is released whilst dragging, then deactivate dragging
        {
            dragging = false;
            drumTrack.tagLocking(false);
        }
        else if(dragging)
        {
            float mouseX = mousePosition.x;
            float mouseY = mousePosition.y;

            float theta = Main.findAngle(mouseX, mouseY);
            drumTrack.moveBeat(beatNum, theta);
            //Debug.Log("Dragging");
        }


    }

    public void setUp(DrumTrack d, int b)
    {

        drumTrack = d;
        beatNum = b;
        dragging = false;
    }
}
