using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public enum GameMode {
    idle,
    playing,
    levelEnd
}
public class SlingshotShooter : MonoBehaviour
{
    static private SlingshotShooter S;
    [Header("Defined in inspector")]
    public Text uitLevel; //UIText_Level
    public Text uitShots; //UIText_Shots
    public Text uitButton; //Button UIButton_View name
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Defined dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //FollowCam Mode

    private void Start() {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void StartLevel() {
        if(castle!= null) {
            Destroy(castle);
        }

        //remove old projectiles
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos){
            Destroy(pTemp);
        }

        //create new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //Set up default camera setting
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    private void UpdateGUI() {
        uitLevel.text = "Poziom: " + (level + 1) + " z " + levelMax;
        uitShots.text = "Strzały: " + shotsTaken;
    }

    private void Update() {
        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet) {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    private void NextLevel() {
        level++;
        if(level == levelMax) {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "") {
        if(eView == "") {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing) {
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
        
    }

    public static void ShotFired() {
        S.shotsTaken++;
    }
}
