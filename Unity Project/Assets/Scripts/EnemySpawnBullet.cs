using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class EnemySpawnBullet : MonoBehaviour {
    public GameObject bullet;
    public float BulletPerSec = 1.0f;
    private Vector3 SpawnTarget;
    private float timeStamp;
    private GameObject light;
    private GameObject GunAnimation;
    public AudioClip soundeffect;
	// Use this for initialization
	void Start () {
        timeStamp = 1 / BulletPerSec;
        //var a = gameObject.transform.parent.Find("Point light").gameObject;
        //if (a != null)
        //{
        //    light = a;
        //} else
        //{
        //    Debug.Log("cant find light object for gun");
        //}
	}
	
	// Update is called once per frame
	void Update () {

	}
    void Spawn(){
        GameObject newBullet;
        newBullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        newBullet.GetComponent<EnemyBulletFly>().SetflyingTo(SpawnTarget);
        if (GunAnimation != null)
        {
            GunAnimation.particleSystem.Play();
            Invoke("StopAnimation",0.5f);
        }
        Invoke("flashLight", 0.0f);
        //audio.PlayOneShot(soundeffect,0.7f);
        AudioSource.PlayClipAtPoint(soundeffect, transform.position);
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
    public void flashLight(){
        //light.SetActive(true);
        Invoke("flashLightDisable", 2.0f);
    }
    public void flashLightDisable(){
        //light.SetActive(false);
    }
}
