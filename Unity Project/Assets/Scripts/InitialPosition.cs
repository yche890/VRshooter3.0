using UnityEngine;
using System.Collections;
using OVR;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class InitialPosition : MonoBehaviour {
    public GameObject player;
	//public GameObject splines;
	//public float timeTaken;
    public float delay;
    //public GameObject testEnemy;
    private bool canGo = false;
    //public static Hmd HMD;
    private static bool gameStart = false;
    private static float countDown;
    public GameObject[] toDestroy;
    public GameObject forwardDir;

    void Start () {
        //testEnemy.GetComponentInChildren<EnemyGunScript>().setEnemyActive(true);
        player.GetComponentInChildren<KinectInput>().setRestrict(3);
        countDown = delay;
        if (!canGo){
            Invoke("startMove", delay);
            canGo = true;

        }

    }
    void Update(){
        countDown -= Time.deltaTime;
        if (!canGo){
            if (KinectInput.GetResume()){
                CancelInvoke("startMove");
                startMove();
                canGo = true;
            }
        }
    }
    // the first pickup
	void OnTriggerEnter(Collider collider){
        //Debug.Log("InitialPickup hit : " + collider.gameObject.name);
		if (collider.gameObject.tag == "Player"  ) {
            //testing

		}
	}

    public void startMove(){
        Debug.Log("Game start!");
        gameStart = true;
        //reset the orientation when start game
        Vector3 fdRotation = forwardDir.transform.localEulerAngles;
        fdRotation.y = 0;
        forwardDir.transform.localEulerAngles = fdRotation;

        //HMD = Hmd.GetHmd();
        //HMD.DismissHSWDisplay();
        //KinectControlScript.AddIndexByOne();
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
    public static bool isGameStarted(){
        return gameStart;
    }
    public static float getCountDown(){
        return countDown;

    }
}
