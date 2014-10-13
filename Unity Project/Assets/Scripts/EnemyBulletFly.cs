using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class EnemyBulletFly : MonoBehaviour {
    public GameObject explosion;
    public float moveSpeed = 30f;
    public Vector3 flyingTo;
    public float slowDownParameter;
    private Vector3 dir;
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
	void Update () {
        transform.Translate(dir * Time.deltaTime * moveSpeed);
        Debug.DrawLine(transform.position, transform.position + dir);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 5.5f))
        {
            if (hit.collider.gameObject.tag == "Player" && !hasSlowed )
            {
//                Debug.Log("slow: ");
                Time.timeScale = slowDownParameter;
                hasSlowed = true;
                Invoke("resumeTime" , slowDownParameter);
            }
        }


	}
    void resumeTime(){
        Time.timeScale = 1.0f;
    }
    void DestroyBullet(){
        Invoke("Boom", 0);
        Invoke("resumeTime" , 0.0f);
        Destroy (gameObject, 0.001f);
    }
    
    void Boom () {

    }
    public void SetflyingTo(Vector3 spawntarget){
        flyingTo = spawntarget;
    }
}
