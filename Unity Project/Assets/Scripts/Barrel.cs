using UnityEngine;
using System.Collections;
/************************************************************************************

This file control the event of barrel explosion 
Auther:  Yang Chen 

************************************************************************************/
public class Barrel : MonoBehaviour {
    // explosion effect
    public GameObject explosion;
    // explosion audio clip
    public AudioClip explosionEffect;
    // the enemy killed by the barrel
    public GameObject enemyToKill;
    // activate the barrel when player arrives the battle position
    private bool activated =false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /*********
     * When player's bullet hit barrel explode
     */
    void OnCollisionEnter(Collision c){
        //Debug.Log("player collider:" + c.ToString());
        if (c.gameObject.tag == "PlayerBullet" && activated)
        {
           
           
            Boom ();

        }
    }
    /*********
     * Invoke explosion effect, kill the coresponding enemy
     */
    void Boom () {

        Instantiate(explosion, transform.position ,Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionEffect, transform.position);
        Invoke("Destroy", 0.5f);
        Destroy(enemyToKill, 1.0f);
        HeadsUpDisplay.enemyKilled();
    }
    /// <summary>
    /// Destroy this barrel.
    /// </summary>
    void Destroy(){
        Destroy(gameObject);
    }
    /// <summary>
    /// Activated when the player arrives the battle position
    /// </summary>
    public void Activate(){
        activated = true;
        //Debug.Log("I am enabled" + gameObject.name);
    }
}
