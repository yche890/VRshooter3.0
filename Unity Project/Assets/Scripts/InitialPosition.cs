using UnityEngine;
using System.Collections;
using OVR;
/************************************************************************************

This script initialize player's basic data beside the police car
Detailed comment can be found in BattlePosition.cs
Auther:  Yang Chen

************************************************************************************/
public class InitialPosition : MonoBehaviour {
    /// <summary>
    /// The player.
    /// </summary>
    public GameObject player;
    /// <summary>
    /// The delay between calibration done and game started.
    /// </summary>
    public float delay;
    /// <summary>
    /// Whether the spline has been activated.
    /// </summary>
    private bool splineActivated = false;
    /// <summary>
    /// The game started or not.
    /// </summary>
    private static bool gameStart = false;
    /// <summary>
    /// The calibration is done or not.
    /// </summary>
    private bool calibrationDone = false;
    /// <summary>
    /// The count down counter.
    /// </summary>
    private static float countDown;
    /// <summary>
    /// Mirror to destroy.
    /// </summary>
    public GameObject[] toDestroy;


    void Start () {
        //testEnemy.GetComponentInChildren<EnemyGunScript>().setEnemyActive(true);
        player.GetComponentInChildren<KinectInput>().setRestrict(3);
        countDown = delay;


    }
    void Update(){
        calibrationDone = KinectInput.IsGotInitialData();
        if (calibrationDone)
        {
            countDown -= Time.deltaTime;
            if (!splineActivated && countDown <= 0.0f)
            {
               //CancelInvoke("startMove");
               startMove();
               splineActivated = true;

            }
        }
    }
    // the first pickup
    /*
	void OnTriggerEnter(Collider collider){
        //Debug.Log("InitialPickup hit : " + collider.gameObject.name);
		if (collider.gameObject.tag == "Player"  ) {
            //testing

		}
	}*/

    public void startMove(){
        Debug.Log("Game start!");
        gameStart = true;
        OVRDevice.HMD.DismissHSWDisplay();
        player.GetComponent<SplineController>().enabled = true;
        player.GetComponent<SplineInterpolator>().enabled = true;

        //player.GetComponent<SplineController>().SplineRoot = splines;
        //player.GetComponent<SplineController>().Duration = timeTaken;
        player.GetComponent<SplineController>().StartSpline();
        foreach (GameObject g in toDestroy)
        {
            Destroy(g, 0.5f);
        }
    }
    /// <summary>
    /// Ises the game started.
    /// </summary>
    /// <returns><c>true</c>, if game started was ised, <c>false</c> otherwise.</returns>
    public static bool isGameStarted(){
        return gameStart;
    }
    /// <summary>
    /// Gets the count down counter by headup display.
    /// </summary>
    /// <returns>The count down.</returns>
    public static float getCountDown(){
        return countDown;

    }
}
