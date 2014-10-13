using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class SpawnBullet : MonoBehaviour {
    public GameObject bullet;
    public float BulletPerSec = 1.0f;
    private Vector3 SpawnTarget;
    private float timeStamp;
    public AudioClip soundeffect;
    private GameObject GunAnimation;
	// Use this for initialization
	void Start () {
        timeStamp = 1 / BulletPerSec;
	}
	
	// Update is called once per frame
	void Update () {

	}
    void Spawn(){
        GameObject newBullet;
        newBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        newBullet.GetComponent<BulletFly>().SetflyingTo(SpawnTarget);
        if (GunAnimation != null)
        {
            GunAnimation.particleSystem.Play();
            Invoke("StopAnimation",0.5f);
            AudioSource.PlayClipAtPoint(soundeffect, transform.position);
        }
    }
    void StopAnimation(){
        GunAnimation.particleSystem.Stop();
    }
    public void SetSpawnTarget(Vector3 spawntarget){
        SpawnTarget = spawntarget;
    }

    public void startShooting(){
        startShooting(0.1f, timeStamp);
    }
    public void stopShooting(){
        CancelInvoke("Spawn");
    }

    public void startShooting(float delay, float period){
        InvokeRepeating("Spawn", delay, period);

    }

    public void setAnimation(GameObject ani){
        GunAnimation = ani;
        GunAnimation.particleSystem.loop = false;
    }
}
