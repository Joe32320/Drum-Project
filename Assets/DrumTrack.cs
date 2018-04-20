using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrumTrack : MonoBehaviour {

    private List<GameObject> triangleMasks; //List of the triangle masks used to create the beat wedges
    private List<GameObject> beatCircles; // GameObject of each beat circle in the drum track
    private List<GameObject> lines; // List of line GameObjects
    private List<GameObject> tags; // List of tag GameObjects 
    private GameObject circleMask; //maks that masks the inner radius of the beat circles
    private AudioSource source; // The sound file of the drum to be played

    private GameObject circleLineMask; // Mask to help create the dividing line between drumtracks
    private GameObject circleLine; //Circle used to draw line between drum tracks


    private Dictionary<GameObject, float> lineAngles; //
    private List<float> volumeLevels; //List of each beats volume level
    private float minimumRadius; //Inner radius of the ring
    private float maximumRadius; // Outer radius of the ring

    private bool beingEdited; //Whether the track is being edited in edit mode

    private float offsetAngle; //Angle whole drumtrack has been rotated by in degrees

    private Color color; //The color of the beat circles
    private float lineWidth; 
    private bool tagLock; // Whether a tag is currently being dragged, to be used to disable behaviour whilst this is happening
    private int activeTag; // The number of the beat assoicated with the tag being dragged


	// Use this for initialization
	void Start () {

        
        
        
        tags = new List<GameObject>();

        
        
        offsetAngle = 0f;
		
	}

    //Acts in some way as the constructor, supplying the object with all the referecnces it needs to set up
    public void setUp(int beats, GameObject maskSprite, GameObject circleMaskSprite, GameObject circleSprite,int layerID, Color nColor, float nLineWidth, GameObject lineSprite, AudioClip soundFile)
    {

        source = this.gameObject.AddComponent<AudioSource>();
        source.clip = soundFile;

        GameObject masksObject = new GameObject();
        masksObject.transform.parent = this.transform;
        lineWidth = nLineWidth;
        color = nColor;

        triangleMasks = new List<GameObject>();
        beatCircles = new List<GameObject>();
        lines = new List<GameObject>();
        lineAngles = new Dictionary<GameObject, float>();
        float standardAngle = (float)360f / beats;
        
        volumeLevels = new List<float>();
        //Debug.Log(standardAngle);

        circleMask = Instantiate(circleMaskSprite, new Vector3(0, 0, 0), Quaternion.identity);
        circleMask.transform.localScale = new Vector3(0, 0, 0);
        circleMask.transform.parent = this.transform;
        circleMask.GetComponent<SpriteMask>().frontSortingLayerID = layerID;
        circleMask.GetComponent<SpriteMask>().backSortingLayerID = layerID;
        circleMask.GetComponent<SpriteMask>().frontSortingOrder = 100;
        circleMask.GetComponent<SpriteMask>().backSortingOrder = -100;

        this.createCircleBorder(circleMaskSprite, circleSprite, layerID);

        for (int i = 0; i < beats; i++)
        {
            this.setUpBeatCircle(standardAngle, layerID, i, maskSprite, circleSprite);
            this.setUpLines(lineSprite, (float)i * standardAngle, layerID, i);
        }



    }

    //Sets up all the beat lines
    void setUpLines(GameObject lineSprite, float angle, int layerID, int i)
    {
        GameObject line = Instantiate(lineSprite, Vector3.zero, Quaternion.AngleAxis(-angle, Vector3.forward));
        line.transform.parent = this.transform;
        SpriteRenderer sR = line.transform.GetChild(0).GetComponent<SpriteRenderer>();
        sR.sortingLayerID = layerID;
        sR.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        sR.sortingOrder = 99;

        line.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        line.transform.GetChild(1).GetComponent<TagCollider>().setUp(this, i);

        lines.Add(line);
        lineAngles.Add(line, angle);
    }

    //Sets up a beat circle
    void setUpBeatCircle(float standardAngle, int layerID, int i, GameObject maskSprite, GameObject circleSprite)
    {
        float angle = -(standardAngle / 2f) - i * standardAngle + 360;
        float[] triMaskAngles = DrumTrack.maskTriangleLocations(angle, standardAngle);
        

        GameObject firstTriMask = Instantiate(maskSprite, new Vector3(0, 0, 0), Quaternion.identity);
        firstTriMask.transform.parent = this.transform;
        firstTriMask.transform.rotation = Quaternion.AngleAxis(triMaskAngles[1], Vector3.forward);
        firstTriMask.transform.GetChild(0).transform.localScale = new Vector3(triMaskAngles[2], 10, 1);
        firstTriMask.transform.GetChild(0).GetComponent<SpriteMask>().frontSortingLayerID = layerID;
        firstTriMask.transform.GetChild(0).GetComponent<SpriteMask>().backSortingLayerID = layerID;
        firstTriMask.transform.GetChild(0).GetComponent<SpriteMask>().frontSortingOrder = i;
        firstTriMask.transform.GetChild(0).GetComponent<SpriteMask>().backSortingOrder = i - 1;

        triangleMasks.Add(firstTriMask);

        GameObject secondTriMask = Instantiate(maskSprite, new Vector3(0, 0, 0), Quaternion.identity);
        secondTriMask.transform.parent = this.transform;
        secondTriMask.transform.rotation = Quaternion.AngleAxis(triMaskAngles[0], Vector3.forward);
        secondTriMask.transform.GetChild(0).transform.localScale = new Vector3(triMaskAngles[2], 10, 1);
        secondTriMask.transform.GetChild(0).GetComponent<SpriteMask>().frontSortingLayerID = layerID;
        secondTriMask.transform.GetChild(0).GetComponent<SpriteMask>().backSortingLayerID = layerID;
        secondTriMask.transform.GetChild(0).GetComponent<SpriteMask>().frontSortingOrder = i;
        secondTriMask.transform.GetChild(0).GetComponent<SpriteMask>().backSortingOrder = i - 1;

        triangleMasks.Add(secondTriMask);

        //GameObject circleMask = Instantiate(circleMaskSprite, new Vector3(0, 0, 0), Quaternion.identity);

        GameObject beatCircle = Instantiate(circleSprite, new Vector3(0, 0, 0), Quaternion.identity);
        beatCircle.transform.parent = this.transform;
        beatCircle.GetComponent<SpriteRenderer>().sortingLayerID = layerID;
        beatCircle.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        beatCircle.GetComponent<SpriteRenderer>().sortingOrder = i;
        beatCircle.GetComponent<SpriteRenderer>().color = color;
        float rand = Random.value;

        beatCircle.transform.localScale = new Vector3(rand * 10, rand * 10, 1);
        beatCircles.Add(beatCircle);
        volumeLevels.Add(rand);
    }

    //Changes all the beat sizes depending on the angles the beat lines are at
    void changeBeatSize()
    {
        
        for(int i = 0; i < beatCircles.Count; i++)
        {
            float precedingLine = 360 - lines[i].transform.eulerAngles.z;
            float nextLine = 360 - lines[(i + 1) % lines.Count].transform.eulerAngles.z;

            float wedgeAngle = 0;
            float centreAngle = 0;
            if(precedingLine > nextLine)
            {
                wedgeAngle = nextLine - precedingLine + 360;
                centreAngle = (precedingLine - 360) + (wedgeAngle / 2f);
            }
            else
            {
                wedgeAngle = nextLine - precedingLine;
                centreAngle = precedingLine + (wedgeAngle / 2f);
            }

            float[] m = maskTriangleLocations(centreAngle, wedgeAngle);
            GameObject firstTriMask = triangleMasks[i * 2];
            GameObject secondTriMask = triangleMasks[(i * 2) + 1];

            firstTriMask.transform.rotation = Quaternion.AngleAxis(m[1], Vector3.back);
            firstTriMask.transform.GetChild(0).transform.localScale = new Vector3(m[2], 10, 1);

            secondTriMask.transform.rotation = Quaternion.AngleAxis(m[0], Vector3.back);
            secondTriMask.transform.GetChild(0).transform.localScale = new Vector3(m[2], 10, 1);

        }



    }

    //Creates the circluar line border between different drum tracks, and the edge of the whole circle graphic
    void createCircleBorder(GameObject circleMaskSprite, GameObject circleSprite, int layerID)
    {
        circleLineMask = Instantiate(circleMaskSprite, new Vector3(0, 0, 1), Quaternion.identity);
        circleLineMask.transform.localScale = new Vector3(10-lineWidth, 10-lineWidth, 1);
        circleLineMask.transform.parent = this.transform;
        circleLineMask.GetComponent<SpriteMask>().frontSortingLayerID = layerID;
        circleLineMask.GetComponent<SpriteMask>().backSortingLayerID = layerID;
        circleLineMask.GetComponent<SpriteMask>().frontSortingOrder = -101;
        circleLineMask.GetComponent<SpriteMask>().backSortingOrder = -102;

        circleLine = Instantiate(circleSprite, Vector3.zero, Quaternion.identity);
        circleLine.transform.localScale = new Vector3(10, 10, 1);
        circleLine.transform.parent = this.transform;
        circleLine.GetComponent<SpriteRenderer>().sortingLayerID = layerID;
        circleLine.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        circleLine.GetComponent<SpriteRenderer>().sortingOrder = -102;
        circleLine.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
    }


    //Resizes the beat circles and circle masks to produce different ring patterns
    public void resize(float minR, float maxR, bool permanentResized)
    {

        if(minR > maxR)
        {
            Debug.LogError("Passed variable out of range");
        }

        if (permanentResized)
        {
            minimumRadius = minR;
            maximumRadius = maxR;
            Debug.Log("Passed: "+ minimumRadius + " : " + maximumRadius );
        }
        

        float rangeR = maxR - minR-lineWidth;
        circleMask.transform.localScale = new Vector3(minR, minR, 1);

        for(int i = 0; i < beatCircles.Count; i++)
        {
            float size = minR + (volumeLevels[i] * rangeR);
            beatCircles[i].transform.localScale = new Vector3(size, size);
        }

        for(int i = 0; i < lines.Count; i++)
        {
            lines[i].transform.localScale = new Vector3(1f, maxR);
        }

        circleLineMask.transform.localScale = new Vector3(maxR - lineWidth, maxR - lineWidth);
        circleLine.transform.localScale = new Vector3(maxR, maxR);




    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //positionAngle is the centre position of the wedge, the wedge angle is the angle of the wedge itself
    //Produces the two angles where each triangle mask should be located and how large they should be
    public static float[] maskTriangleLocations(float positionAngle, float wedgeAngle)
    {

        float firstAngle = 90f + (wedgeAngle / 4f) + positionAngle;
        float secondAngle = firstAngle + 180f - (wedgeAngle / 2);

        firstAngle = (firstAngle +180) % 360;
        secondAngle = (secondAngle +180)  % 360;

        float maskWedgeAngle = (360f - wedgeAngle) / 2f;
        
        //Debug.Log("First Angle: " + firstAngle + " Second Angle: " + secondAngle + " Mask Wedge Angle: " + maskWedgeAngle);

        float[] angleArray = new float[3];
        angleArray[0] = firstAngle;
        angleArray[1] = secondAngle;
        angleArray[2] = (20 * Mathf.Tan(Mathf.Deg2Rad * maskWedgeAngle / 2f)) * 0.866025f;

        //Debug.Log(angleArray[0] + ":" + angleArray[1] + ":" + angleArray[2] + ":" + positionAngle + ":" + wedgeAngle);

        return angleArray;
    }

    public float getMinimumRadius()
    {
        return minimumRadius;
    }

    public float getMaximumRadius()
    {
        return maximumRadius;
    }
    
    //Hides all graphics associated with the drumtrack
    public void hide()
    {
        for(int i = 0; i < beatCircles.Count; i++)
        {
            beatCircles[i].GetComponent<SpriteRenderer>().enabled = false;
            
        }

        for(int i = 0; i < lines.Count; i++)
        {
            lines[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }

        circleLine.GetComponent<SpriteRenderer>().enabled = false;
    }
    //Shows all graphics associated with the drumtrack
    public void show()
    {
        for (int i = 0; i < beatCircles.Count; i++)
        {
            beatCircles[i].GetComponent<SpriteRenderer>().enabled = true;
        }
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
        circleLine.GetComponent<SpriteRenderer>().enabled = true;
    }

    //Resizes the drumtrack to allow editing, and activates tags to allow users to drag beat lines about
    public void enterEditMode()
    {
        resize(1f, 10f, false);
        beingEdited = true;
        for(int i = 0; i < lines.Count; i++)
        {
            lines[i].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    //Resizes the track back down to it's previous size before entering edit mode
    public void exitEditMode()
    {

        if(beingEdited == true)
        {
            resize(minimumRadius, maximumRadius, false);
            Debug.Log(minimumRadius + ":" + maximumRadius);
            beingEdited = false;

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        
    }

    public void editVolume(float angle, float r)
    {
        if (r > 10)
        {
            r = 10;
        }
        r = r - 1;
        float increment = Mathf.RoundToInt(r / 9f*5f)/5f;

        List<float> lAngles = new List<float>();

        for(int i = 0; i < lines.Count; i++)
        {
            lAngles.Add((360-getLineAngle(i))%360f);
        }
        lAngles.Sort();
        int beatNum = -2;
        for(int i = 0; i < lines.Count; i++)
        {
            float comparedAngle = lAngles[i];
            if(angle < comparedAngle)
            {
                beatNum = i - 1;
                break;
            }
        }
        if (beatNum == -1)
        {
            beatNum = lines.Count - 1;
        }
        else if(beatNum == -2)
        {
            beatNum = 0;
        }


        float range = 9 - lineWidth;
        float size = 1 + (increment * range);
        volumeLevels[beatNum] = increment;

        beatCircles[beatNum].transform.localScale = new Vector3(size, size);
    }

    public int getBeats()
    {
        return beatCircles.Count;
    }

    public float getLineAngle(int lineNum)
    {
        return lines[lineNum].transform.rotation.eulerAngles.z;
    }

    public void playSound(int beatNum)
    {
        source.volume = volumeLevels[beatNum] - 0.05f;
        source.Play();
    }

    
    void findBeatCentresAndAngles(List<float> lAngles)
    {

        List<float> centreAngles = new List<float>();
        List<float> wedgeAngles = new List<float>();

        for (int i = 0; i < lAngles.Count; i++)
        {
            float currentAngle = lAngles[i];
            float nextAngle = lAngles[(i + 1) % lAngles.Count];
            if (nextAngle < currentAngle)
            {
                nextAngle += 360f;
            }

            float centreAngle = -((nextAngle - currentAngle) / 2f + currentAngle)+360;
            

            float wedgeAngle = nextAngle - currentAngle;

            //float[] maskAngles = maskTriangleLocations(centreAngle, wedgeAngle);
            //triangleMasks[i * 2].transform.rotation = Quaternion.AngleAxis(maskAngles[1], Vector3.forward);
            //triangleMasks[i * 2].transform.GetChild(0).transform.localScale = new Vector3(maskAngles[2], 10, 1);

            //triangleMasks[(i * 2) + 1].transform.rotation = Quaternion.AngleAxis(maskAngles[0], Vector3.forward);
            //triangleMasks[(i * 2) + 1].transform.GetChild(0).transform.localScale = new Vector3(maskAngles[2], 10, 1);

            lines[i].transform.rotation = Quaternion.AngleAxis(lAngles[i], Vector3.back);
        }

    }



    public void moveBeat(int beatNum, float angle)
    {

        if(angle < 0 || angle > 360)
        {
            Debug.LogError("Out of bounds");
        }

        

        GameObject line = lines[beatNum];
        int b = beatNum - 1;
        if (beatNum == 0)
        {
            b = lines.Count - 1;
        }

        float previousLine= 360-lines[b].transform.rotation.eulerAngles.z;
        float nextLine = 360-lines[(beatNum + 1) % lines.Count].transform.rotation.eulerAngles.z;

        //Debug.Log(angle + ":" + beatNum + ":" + previousLine + ":" + nextLine);

        if(lines.Count <= 2)
        {
            line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
        else if (previousLine > nextLine)
        {
            if(angle < nextLine || angle > previousLine)
            {
                line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            }
        }
        else
        {
            if(angle < nextLine && angle > previousLine)
            {
                line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            }
        }

        changeBeatSize();

        

        
    }


    public void tagLocking(bool b)
    {
        
    }

    




}

public class LinesComparer : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {

        if (360 - x.transform.rotation.eulerAngles.z > 360 - y.transform.rotation.eulerAngles.z)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
