using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //Singleton

    [Header("Set in Inspector")]
    public Text uitLevel; //UIText_Level text
    public Text uitShots; //UIText_Shots text
    public Text uitButton; //The text on the UI button
    public Vector3 castlePos; //The place to put the castle
    public GameObject[] castles; //An Array of all the Castles

    [Header("Set dynamically")]
    public int level; //current level
    public int levelMax; //# of levels
    public int shotsTaken;
    public GameObject castle; //current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //FollowCam mode

    // Start is called before the first frame update
    void Start()
    {
        S = this; //define Singleton

        level = 0;
        levelMax = castles.Length;
        showing = "Show Slingshot";
        StartLevel();
    }

    void StartLevel(){
         //checks to see if there is a castle already on screen
        if (castle != null)
        {
            Destroy(castle); //destroys the old castle
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp); //destroys any old projectiles
        } //end foreach

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //resetting camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    
    // Update is called once per frame    
    void Update()
    {
        UpdateGUI();

        if ((mode == GameMode.playing) && Goal.goalMet) //check level end
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both"); //zoom out
            Invoke("NextLevel", 2f);
        }
    }//end update

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;

        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }

    } //end SwitchView

    //incriment Shots Taken wherever allowed
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}