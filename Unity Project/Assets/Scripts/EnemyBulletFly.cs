using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
When an enemy bullet is about the hit the player, time slows down for player to dodge
Auther:  Yang Chen
Detailed comment can be found in player's bullet

************************************************************************************/
public class EnemyBulletFly : MonoBehaviour {
    public GameObject explosion;
    public float moveSpeed = 30f;
    public Vector3 flyingTo;
    // the parameter of time slow down, e.g. 0.1 means the time is 10 times slower
    public float slowDownParameter;
    private Vector3 dir;
    // the bullet will only slow down the time scale once
    private bool hasSlowed = false;

	// Use this for initialization
	void Start () {
        dir = flyingTo - transform.position;
        dir.Normalize();
        Invoke ("DestroyBullet", 10f);
	}
    void OnCollisionEnter(Collision c){
        //Debug.Log("I hit" + c.gameObject.name);
        Invoke ("DestroyBullet",0.0f);
    }

	// Update is called once per frame
    /*
     * When the distance between enemy bullet and player is smaller than a certain value, 
     * the game's time is slowed down by 10 times
     * so player can dodge before being hit
     */
	void Update () {
        transform.Translate(dir * Time.deltaTime * moveSpeed);
        Debug.DrawLine(transform.position, transform.position + dir);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 5.5f))
        {
            if (hit.collider.gameObject.tag == "Player" && !hasSlowed )
            {
//Debug.Log("slow: ");
                Time.timeScale = slowDownParameter;
                hasSlowed = true;
                Invoke("resumeTime" , slowDownParameter);
            }
        }


	}
    // resume the game when time exceeded
    void resumeTime(){
        Time.timeScale = 1.0f;
    }
    void DestroyBullet(){
        Invoke("Boom", 0);
        Invoke("resumeTime" , 0.0f);
        Destroy (gameObject, 0.001f);
    }
    
    void Boom () {
        // there is no blood effect
    }
    /// <summary>
    /// gun script can set the bullet destination when invoke this bullet
    /// </summary>
    /// <param name="spawntarget">The destination of bullet</param>
    public void SetflyingTo(Vector3 spawntarget){
        flyingTo = spawntarget;
    }
}
