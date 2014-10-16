using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an player's bullet
Auther:  Yang Chen

************************************************************************************/
public class BulletFly : MonoBehaviour {
    // when bullet hit something
    public GameObject explosion;
    // speed of bullet
    public float moveSpeed = 30f;
    /// <summary>
    /// the destination of bullet
    /// </summary>
    public Vector3 flyingTo;
    // the normalized direction
    private Vector3 dir;
	// Use this for initialization
    // initialize the direction
	void Start () {
        dir = flyingTo - transform.position;
        dir.Normalize();
        Invoke ("DestroyBullet", 20.0f);
	}
    // when bullet hit something, destroy
    void OnCollisionEnter(Collision c){
        //Debug.Log("I hit" + c.gameObject.name);
        Invoke ("DestroyBullet", 0.0f);
    }

	// move the bullet per frame
	void Update () {
        transform.Translate(dir * Time.deltaTime * moveSpeed);
	}
    void DestroyBullet(){

        Destroy (gameObject, 0.001f);
    }
    /// <summary>
    /// gun script can set the bullet destination when invoke this bullet
    /// </summary>
    /// <param name="spawntarget">The destination of bullet</param>
    public void SetflyingTo(Vector3 spawntarget){
        flyingTo = spawntarget;
    }
}
