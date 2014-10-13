using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {
    public GameObject explosion;
    public AudioClip explosionEffect;
    public GameObject enemyToKill;
    private bool activated =false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision c){
        //Debug.Log("player collider:" + c.ToString());
        if (c.gameObject.tag == "PlayerBullet" && activated)
        {
           
           
            Boom ();

        }
    }

    void Boom () {

        Instantiate(explosion, transform.position ,Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionEffect, transform.position);
        Invoke("Destroy", 0.5f);
        Destroy(enemyToKill, 1.0f);
        HeadsUpDisplay.enemyKilled();
    }
    void Destroy(){
        Destroy(gameObject);
    }
    public void Activate(){
        activated = true;
        Debug.Log("I am enabled" + gameObject.name);
    }
}
