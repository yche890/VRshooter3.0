using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen

************************************************************************************/
public class BulletFly : MonoBehaviour {
    public GameObject explosion;
    public float moveSpeed = 30f;
    public Vector3 flyingTo;
    private Vector3 dir;
	// Use this for initialization
	void Start () {
        dir = flyingTo - transform.position;
        dir.Normalize();
        Invoke ("DestroyBullet", 20.0f);
	}
    void OnCollisionEnter(Collision c){
        //Debug.Log("I hit" + c.gameObject.name);
        Invoke ("DestroyBullet", 0.0f);
    }

	// Update is called once per frame
	void Update () {
        transform.Translate(dir * Time.deltaTime * moveSpeed);
	}
    void DestroyBullet(){

        Destroy (gameObject, 0.001f);
    }

    public void SetflyingTo(Vector3 spawntarget){
        flyingTo = spawntarget;
    }
}
