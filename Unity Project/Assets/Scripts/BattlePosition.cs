using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class BattlePosition: MonoBehaviour {
    public GameObject player;
	public GameObject splines;
	public float timeTaken;
    public float delay;
    public GameObject[] enemies;
    private bool canGo = false;
    private bool isSetRightMost = false;
    public GameObject explosion;
    public AudioClip explosionEffect;
    public GameObject objectToDestroy;
    private bool pickedUp = false;
    public GameObject barrelToEnable;
    public int dodgeRestrict;   //1:only left  2:only right 3: no left or right 0: no limit
    void Start(){
        foreach (GameObject g in enemies)
        {
            if (g != null)
            {
                g.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }
    // the first pickup
	void OnTriggerEnter(Collider collider){

        if (collider.gameObject.tag == "Player") {
            pickedUp = true;
            //Debug.LogError("pickup trigger collide");
            player.GetComponentInChildren<KinectInput>().setRestrict(dodgeRestrict);
            if (!isSetRightMost){
                //player.GetComponentInChildren<KinectControlScript>().setRightMost(gameObject.transform.position);
                isSetRightMost = true;
            }
            foreach (GameObject g in enemies){
                if(g != null){
                    g.GetComponentInChildren<EnemyGunScript>().setEnemyActive(true);
                    g.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                    g.GetComponentInChildren<MeshRenderer>().enabled = true;
                }
            }
            barrelToEnable.GetComponent<Barrel>().Activate();
		}
	}

    void Update(){
        if (checkEnemyAllDie() && pickedUp)
        {
            //Debug.LogError("enemy all dead");
            if (!canGo)
            {
                if (explosion != null){
                    Invoke("Boom", delay + 1.0f);
                }
                Invoke("startMove", delay);
                canGo = true;
            }
        }
    }
    public void startMove(){
        //player.GetComponent<SplineController>().enabled = true;
        //KinectControlScript.AddIndexByOne();

        player.GetComponent<SplineController>().SplineRoot = splines;
        player.GetComponent<SplineController>().Duration = timeTaken;

        player.GetComponent<SplineController>().StartSpline();
        pickedUp = false;
    }
    void Boom () {
        Vector3 pos = splines.transform.FindChild("1").position;
        Instantiate(explosion,pos ,Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionEffect, transform.position);
        Destroy(objectToDestroy, 0.0f);
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
}
