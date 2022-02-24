using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    /*** Variables***/
    static public GameObject POI; //The static point of interest
    
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // desired Z pos of Camera



    void Awake()
    {
        camZ = this.transform.position.z; //sets Z
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (POI == null) return; //if there is no POI exit method
        Vector3 destination;
        //Vector3 destination = POI.transform.position; //get destination of POI
        //destination.x = Mathf.Max(minXY.x, destination.x);
        //destination.y = Mathf.Max(minXY.y, destination.y);
       // destination = Vector3.Lerp(transform.position,destination,easing);
        

        if (POI == null) //if no POI
        {
            destination = Vector3.zero; //destination = 0
        }
        else
        {
            destination = POI.transform.position;
            if(POI.tag == "Projectile")
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null; //null POI if rigidbody is asleep
                    return; //in next update;

                }//end if

            }//end if (POI.tag == "Projectile")

        }//end else

        //Limit camera's min XY vals
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //interpolate from current camera position toward destination
        
        
        destination.z = camZ;
        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;

    }//end FixedUpdate()
}
