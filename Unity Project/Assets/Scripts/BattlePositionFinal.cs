using UnityEngine;
using System.Collections;
/************************************************************************************

This script initialize enemies when player arrives the final boss position
Detailed comments can be found in BattlePosition.cs
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class BattlePositionFinal: MonoBehaviour {
    public GameObject player;
	public GameObject splines;
	public float timeTaken;
    public float delay;
    public GameObject[] enemies;
    private bool splineActivated = false;
    public GameObject explosion;
    public AudioClip explosionEffect;
    private static bool gameOver =false;
    private bool pickedUp = false;
    public GameObject barrelToEnable;
    // the final boss
    private GameObject finalBoss;
    /// <summary>
    /// The dodge restrict.
    /// 1:only left-dodge  2:only right-dodge 3: no left or right 0: no limit
    /// </summary>
    public int dodgeRestrict;
    /// <summary>
    /// If the message of "boss is coming" should be displayed
    /// </summary>
    private static bool showBossComing = false;
    /// <summary>
    /// The boss position.
    /// </summary>
    private Vector3 Bosspos;
    /*********
     * Inactivate all enemies when game started
     */
    void Start(){

        foreach (GameObject g in enemies)
        {
            if (g != null)
            {
                g.GetComponentInChildren<MeshRenderer>().enabled = false;
                finalBoss = g;
            }
        }
        Bosspos = finalBoss.transform.position;
    }
    /*********
     * when spline controller bring the player to the new battle position
     * setup left-right dodge restriction, setup right-most position, enemies appear, and activate barrel
     */
	void OnTriggerEnter(Collider collider){

        if (collider.gameObject.tag == "Player") {
            pickedUp = true;
            //Debug.LogError("pickup trigger collide");
            player.GetComponentInChildren<KinectInput>().setRestrict(dodgeRestrict);
            showBossComing = true;
            HeadsUpDisplay.DisableBossComing();
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
            if (!splineActivated)
            {
                if (explosion != null){
                    Invoke("Boom", delay + 1.0f);
                }
                Invoke("startMove", delay);
                splineActivated = true;
            }
            gameOver = true;
        }
    }

    /*********
     * Activate spline controller, player moves back to the police car
     */
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

        Instantiate(explosion,Bosspos ,Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionEffect, transform.position);
    }
    /*********
     * Disable the headup message "boss coming"
     */

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

}
