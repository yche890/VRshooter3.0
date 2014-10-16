using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of the gun muzzle which spawns the bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class SpawnBullet : MonoBehaviour {
    /// <summary>
    /// The bullet.
    /// </summary>
    public GameObject bullet;
    /// <summary>
    /// The number of bullets spawned per sec.
    /// </summary>
    public float BulletPerSec = 1.0f;
    /// <summary>
    /// The target of bullets.
    /// </summary>
    private Vector3 SpawnTarget;
    /// <summary>
    /// The time between 2 bullets.
    /// </summary>
    private float timeStamp;
    /// <summary>
    /// The soundeffect.
    /// </summary>
    public AudioClip soundeffect;
    /// <summary>
    /// The gun animation.
    /// </summary>
    private GameObject GunAnimation;
	// Use this for initialization
	void Start () {
        timeStamp = 1 / BulletPerSec;
	}
	
	// Update is called once per frame
	void Update () {

	}
    /// <summary>
    /// Spawn a bullet flying to the target
    /// </summary>
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
    /// <summary>
    /// Stops the shooting animation.
    /// </summary>
    void StopAnimation(){
        GunAnimation.particleSystem.Stop();
    }
    /// <summary>
    /// Sets the target of bullet.
    /// </summary>
    /// <param name="spawntarget">The global position of target.</param>
    public void SetSpawnTarget(Vector3 spawntarget){
        SpawnTarget = spawntarget;
    }
    /// <summary>
    /// Starts the shooting.
    /// </summary>
    public void startShooting(){
        InvokeBullets(0.1f, timeStamp);
    }
    /// <summary>
    /// Stops the shooting.
    /// </summary>
    public void stopShooting(){
        CancelInvoke("Spawn");
    }
    /// <summary>
    /// Invokes the bullets.
    /// </summary>
    /// <param name="delay">Delay.</param>
    /// <param name="period">Frequency of bullet spawn.</param>
    public void InvokeBullets(float delay, float period){
        InvokeRepeating("Spawn", delay, period);
        
    }
    /// <summary>
    /// Sets the animation of firing.
    /// </summary>
    /// <param name="animation">firing animation.</param>
    public void setAnimation(GameObject animation){
        GunAnimation = animation;
        GunAnimation.particleSystem.loop = false;
    }
}
