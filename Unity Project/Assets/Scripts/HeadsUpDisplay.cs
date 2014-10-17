using UnityEngine;
using System.Collections;

public class HeadsUpDisplay : VRGUI
{
    public GUISkin skin;
    public Texture texture;
    private int healthTemp = 100;
    private static int score = 0;
    private static bool IsBossComing = false;
    int count = 0;

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


        //"<size=40>"++"</size>"
        int dodgeRestrict = KinectInput.getLeftRightRestrict();
        string dodgeHint = "";
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
            GUI.Label(new Rect(w / 2 - 300, h / 2 - 200, 600, 400), "<size=60>Final Boss Coming!</size>");
        }
        if (PlayerBehaviour.IsPlayerDead())
        {
            GUI.Label(new Rect(w / 2 - 300, h / 2 - 200, 600, 400), "<size=60>You Are Killed!</size>");
        }
        if (BattlePositionFinal.IsgameOver())
        {
            GUI.Label(new Rect(w / 2 - 300, h / 2 - 200, 600, 400), "<size=60>Mission Accomplished!</size>");
        }
        if (!KinectInput.IsGotInitialData())
        {
            GUI.Label(new Rect(w / 2 - 300, h - 350, 600, 400), "<size=42>Pre-game calibration for Kinect</size>");
            GUI.Label(new Rect(w / 2 - 300, h - 300, 600, 400), "<size=42>Face to Kinect, show hands beside ears</size>");
        }
        float height = KinectInput.playerHeight;
        GUI.Label(new Rect(0, 0, 300, 200), "<size=42>Height: " + height +"</size>");
        //GUI.Label(new Rect(Screen.width - 200,Screen.height-35,200,80), "+");

        //GUI.DrawTexture(new Rect(0f, 0f, Screen.width,Screen.height), texture);
        //GUI.Label(new Rect(Screen.width/2 - 400, Screen.height - 100, 800, 100), "Press Space For Menu"); 
    }

    public static void enemyKilled(){
        score += 10;
    }
    public static void shoudShowFinalBossMessage(){
        IsBossComing = true;

    }
    public static void DisableBossComing(){
        IsBossComing = false;
    }
}