using UnityEngine;
using System.Collections;
/************************************************************************************

This script initialize enemies when player arrives a new battle position
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class BattlePosition: MonoBehaviour {
    public GameObject player;
    /// <summary>
    /// The path which will be activated when enemies are cleared in this position
    /// </summary>
	public GameObject splines; 
    /// <summary>
    /// The time taken by the spline above
    /// </summary>
	public float timeTaken;
    /// <summary>
    /// when the enemies are cleared, there will be a delay before moving to the next battle position
    /// </summary>
    public float delay;
    /// <summary>
    /// Enemies in this battle position
    /// </summary>
    public GameObject[] enemies;
    // this boolean ensure that, the spline is activated only once.
    private bool splineActivated = false;
    // the right boundary of player's dodging-to-right action, should be re-initialized when player arrives a new position
    private bool isSetRightMost = false;
    // explosion effect
    public GameObject explosion;
    public AudioClip explosionEffect;
    // the object destroyed when activating spline
    public GameObject objectToDestroy;
    // the player is arrived or not
    private bool pickedUp = false;
    // barrel enabled when player arrived
    public GameObject barrelToEnable;
    /// <summary>
    /// The dodge restrict.
    /// 1:only left-dodge  2:only right-dodge 3: no left or right 0: no limit
    /// </summary>
    public int dodgeRestrict;   

    /// <summary>
    /// Inactivate all enemies when game started
    /// </summary>
    void Start(){
        foreach (GameObject g in enemies)
        {
            if (g != null)
            {
                g.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    /// <summary>
    /// when spline controller bring the player to the new battle position, setup left-right dodge restriction, setup right-most position, enemies appear, and activate barrel
    /// </summary>
    /// <param name="collider">Collider.</param>
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

    /// <summary>
    /// If enemies are all died, activate the spline, and some explosion effect activated 
    /// </summary>
    void Update(){
        if (checkEnemyAllDie() && pickedUp)
        {
            //Debug.LogError("enemy all dead");
            if (!splineActivated)
            {
                if (explosion != null){
                    Invoke("Boom", delay + 1.0f);
                }
                Invoke("startMove", delay);
                splineActivated = true;
            }
        }
    }
    /// <summary>
    ///  Activate spline controller, player moves to the next position
    /// </summary>
    public void startMove(){
        //player.GetComponent<SplineController>().enabled = true;
        //KinectControlScript.AddIndexByOne();

        player.GetComponent<SplineController>().SplineRoot = splines;
        player.GetComponent<SplineController>().Duration = timeTaken;

        player.GetComponent<SplineController>().StartSpline();
        pickedUp = false;
    }
    /*********
     * Invoke explosion
     */
    void Boom () {
        Vector3 pos = splines.transform.FindChild("1").position;
        Instantiate(explosion,pos ,Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionEffect, transform.position);
        Destroy(objectToDestroy, 0.0f);
    }

    /// <summary>
    /// Checks the enemy all die.
    /// </summary>
    /// <returns><c>true</c>, if enemy all dead, <c>false</c> otherwise.</returns>
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
