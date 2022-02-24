using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    //*** VARIABLES ***/
    static public ProjectileLine S; //singleton

    [Header("Set in Inspector")]
    public float minDist = .1f;
    

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this; //sets singleton

        line = GetComponent<LineRenderer>(); //ref to it
        //line.SetWidth;
        line.enabled = false; //disable LineRenderer
        points = new List<Vector3>(); //new list
        
    }

    public GameObject poi //Property
    {
        get { 
            return (_poi); }
        set { 
            _poi = value;
            if ( poi != null) //resets everything
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    // Start is called before the first frame update
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position; //add point to line
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        } //end if

        if (points.Count == 0) //if we found the launch point
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; //defined in Slingshot
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;

            //Sets first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            line.enabled = true;
        }//end if
        else
        {
            //normal behavior
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }//end AddPoint

    public Vector3 lastPoint {
        get
        {
            if (points == null)
            {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (poi == null)
        {
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                } else
                {
                    return; //if there wasn't a good POI
                }//end else
            }// end if (FollowCam.POI != null)
            else
            {
                return; //why is this a duplicate?
            }
        }//end if (poi == null)

        AddPoint();
        if(FollowCam.POI == null)
        {
            poi = null; //once followcam.poi is null, local should be null, too.
        }
    }
}