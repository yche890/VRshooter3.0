using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class BattlePositionFinal: MonoBehaviour {
    public GameObject player;
	public GameObject splines;
	public float timeTaken;
    public float delay;
    public GameObject[] enemies;
    private bool canGo = false;
    public GameObject explosion;
    public AudioClip explosionEffect;
    private static bool gameOver =false;
    private bool pickedUp = false;
    public GameObject barrelToEnable;
    private GameObject finalBoss;
    public int dodgeRestrict;   //1:only left  2:only right 3: no left or right 0: no limit
    private static bool showBossComing = false;
    // the first pickup
    void Start(){
        foreach (GameObject g in enemies)
        {
            if (g != null)
            {
                g.GetComponentInChildren<MeshRenderer>().enabled = false;
                finalBoss = g;
            }
        }
    }
	void OnTriggerEnter(Collider collider){

        if (collider.gameObject.tag == "Player") {
            pickedUp = true;
            //Debug.LogError("pickup trigger collide");
            player.GetComponentInChildren<KinectInput>().setRestrict(dodgeRestrict);
            showBossComing = true;
            Invoke("DisableBossComing" , 3.0f);
            foreach (GameObject g in enemies){
                if(g != null){
                    g.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                    g.GetComponentInChildren<EnemyGunScript>().setEnemyActive(true);
                    g.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
            barrelToEnable.GetComponent<Barrel>().Activate();
		}
	}

    void Update(){
        if (checkEnemyAllDie() && pickedUp)
        {
            Debug.LogError("enemy all dead! game over");
            if (!canGo)
            {
                if (explosion != null){
                    Invoke("Boom", delay + 1.0f);
                }
                Invoke("startMove", delay);
                canGo = true;
            }
            gameOver = true;
        }
    }
    public void startMove(){
        pickedUp = false;
        //player.GetComponent<SplineController>().enabled = true;
        player.GetComponentInChildren<KinectInput>().setRestrict(3); //disable left right movement
        player.GetComponent<SplineController>().SplineRoot = splines;
        player.GetComponent<SplineController>().Duration = timeTaken;
        //player.GetComponent<SplineController>().WrapMode = eWrapMode.LOOP;
        player.GetComponent<SplineController>().StartSpline();
    }
    void Boom () {
        Vector3 pos = finalBoss.transform.position;
        Instantiate(explosion,pos ,Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionEffect, transform.position);
    }
    void DisableBossComing(){
        showBossComing = false;
    }
    public bool checkEnemyAllDie(){
        bool result = true;

        foreach (GameObject g in enemies){
            if(g != null){
                result = false;
            }
        }
        return result;
    }
    public static bool IsgameOver(){
        return gameOver;
    }
    public static bool IsBossComing(){
        return showBossComing;
    }

}
