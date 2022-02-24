using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static bool goalMet = false;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile") //checking to see if projectile
        {
            Goal.goalMet = true;
            

            Material mat = GetComponent<Renderer>().material; //changing color
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }

}