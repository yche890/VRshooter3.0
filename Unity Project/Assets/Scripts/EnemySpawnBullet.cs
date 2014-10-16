using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of enemy gun muzzle
Detailed comment can be found in SpawnBullet.cs
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
	// Use this for initialization, setup the frequency of shooting
	void Start () {
        timeStamp = 1 / BulletPerSec;

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
        InvokeBullets(0.1f, timeStamp);
    }
    public void stopShooting(){
        CancelInvoke("Spawn");
    }

    public void InvokeBullets(float delay, float period){
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
