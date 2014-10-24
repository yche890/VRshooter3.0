using UnityEngine;
using System.Collections;
/*
 * This script describe the behaviour of the head-up display
 * Author Henry Lee, Yang Chen
 */
public class HeadsUpDisplay : VRGUI
{
    /// <summary>
    /// GUISkin refers to the layout UI that is utilized to create the HUD
    /// </summary>
    public GUISkin skin;
    /// <summary>
    /// Store the previous value of the health to detect changes
    /// </summary>
    private int healthTemp = 100;
    /// <summary>
    /// Score keeping
    /// </summary>
    private static int score = 0;
    /// <summary>
    /// Whether the head-up display shows the message of "Final boss coming"
    /// </summary>
    private static bool IsBossComing = false;
    /// <summary>
    /// count is used to delay the removal of the on hit indeicator
    /// </summary>
    int count = 0;


    /// <summary>
    /// Create and Display the VR HUD on the Oculus, creates the Health, Score and any other messages to be displayed
    /// </summary>
    public override void OnVRGUI()
    {
        
        GUI.skin = skin;

        if (healthTemp != PlayerBehaviour.life)
        {
            count++;
            //Debug.Log(healthTemp+" has changed to "+PlayerBehaviour.life);
            GUI.TextArea(new Rect(0, 0, Screen.width, Screen.height),"You've been hit!");
            if(count > 20){
                healthTemp = PlayerBehaviour.life;
                count = 0;
            }
        }

        //The left right arrow indicating which left/right dodge is available
        //"<size=40>"++"</size>"
        int dodgeRestrict = KinectInput.getLeftRightRestrict();
        string dodgeHint = "";
        //Dodge Indicators
        if (dodgeRestrict == 0)
        {
            dodgeHint = "<              >";
        } else if (dodgeRestrict == 1)
        {
            dodgeHint = "<               ";
        } else if (dodgeRestrict == 2)
        {
            dodgeHint = "               >";
        } else
        {
            dodgeHint = "                ";
        }
        



        //Display other information
        if (!BattlePositionFinal.IsgameOver())
        {
            GUI.Label(new Rect(Screen.width / 3.0f, Screen.height / 3.0f, 200, 100), "<size=42>" + PlayerBehaviour.life + "</size>");
            GUI.Label(new Rect(Screen.width * (4.0f / 7.0f), Screen.height / 3.0f, 200, 100), "<size=38>Score: " + score + "</size>");
        
        }int w = Screen.width; int h = Screen.height;
        if (!InitialPosition.isGameStarted() && KinectInput.IsGotInitialData())
        {
            float countDownFloat = InitialPosition.getCountDown();
            int countDown = (int)countDownFloat;
            GUI.Label(new Rect(w / 2 - 300, h - 270, 600, 400), "<size=42>Game start in:" + countDown + " seconds</size>");
        } else
        {
            GUI.Label(new Rect(w / 2 - 300, h - 350, 600, 400), "<size=120>" + dodgeHint + "</size>");
        }
        if (IsBossComing)
        {
            GUI.Label(new Rect(w / 2 - 300, h / 2 - 180, 600, 400), "<size=60>Final Boss Coming!</size>");
            //Debug.Log("show final boss msg!");
        }
        if (PlayerBehaviour.IsPlayerDead())
        {
            GUI.Label(new Rect(w / 2 - 300, h / 2 - 200, 600, 400), "<size=60>You Are Killed!</size>");
        }
        if (BattlePositionFinal.IsgameOver())
        {
            GUI.Label(new Rect(w / 2 - 300, h / 2 - 200, 600, 400), "<size=60>Mission Accomplished!</size>");
            GUI.Label(new Rect(Screen.width * (4.0f / 7.0f), Screen.height / 3.0f, 200, 100), "<size=38>Score: " + score + "</size>");
        }
        if (!KinectInput.IsGotInitialData())
        {
            GUI.Label(new Rect(w / 2 - 300, h - 350, 600, 400), "<size=42>Pre-game calibration for Kinect</size>");
            GUI.Label(new Rect(w / 2 - 300, h - 300, 600, 400), "<size=42>Face to Kinect</size>");
            GUI.Label(new Rect(w / 2 - 300, h - 250, 600, 400), "<size=42>Show hands beside ears</size>");
        }
        float height = KinectInput.playerHeight;
        GUI.Label(new Rect(0, 0, 300, 200), "<size=42>Height: " + height +"</size>");

    }

    /// <summary>
    /// Increase score for each enemy killed
    /// </summary>
    public static void enemyKilled(){
        score += 10;
    }
    /// <summary>
    /// set show final message true
    /// </summary>
    public static void shoudShowFinalBossMessage(){
        IsBossComing = true;

    }
    /// <summary>
    /// set show final message true
    /// </summary>
    public static void DisableBossComing(){
        IsBossComing = false;
    }
}