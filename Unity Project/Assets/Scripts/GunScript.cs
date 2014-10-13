using UnityEngine;
using System.Collections;
using OVR;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen

************************************************************************************/
public class GunScript : MonoBehaviour {

    private GameObject currentBulletSpawner;
    private Vector3     cameraPosition;
    private Vector3     crosshairPosition;
    private OVRMainMenu mm;
    private OVRCameraController occ;
    private OVRCrosshair crossHair;
    private bool firing;
    private GameObject GunAnimation;
    private Vector3 bulletSpawnPos;
    private Vector3 bulletTarget;
    // 1: right eye to aim,
    // -1 left eye to aim; 0;
    // 0: both eyes
    public int usingWhichEyeToAim; 
    public AudioClip guneffect;


	// Use this for initialization
	void Start () {

        firing = false;

        var ani = GameObject.Find("swat/M4A1 Sopmod/Particle System");
        if (ani == null)
        {
            Debug.Log("cant find animation");
        } else
        {
            GunAnimation = ani;
        }
        var bs = GameObject.Find("swat/M4A1 Sopmod/BulletSpawn");
        if (bs == null)
        {
            Debug.Log("cant find BulletSpawn");
                //initially dont spawn bullet
        } else
        {
            bs.SetActive(false);
            currentBulletSpawner = bs;
        }

        var pc = GameObject.Find("OurPlayer/OVRPlayerController");
        if (pc == null)
        {
            Debug.Log("cant find player controller");
        } else
        {
            mm = pc.GetComponent<OVRMainMenu>();
            occ = pc.GetComponentInChildren<OVRCameraController>();
        }
	}
	
	// Update is called once per frame
	void Update () {


        crossHair = mm.GetCrosshair();
        cameraPosition = crossHair.GetCameraPosition();
        float ipd = occ.IPD;
        cameraPosition.x -= ipd * usingWhichEyeToAim;

        crosshairPosition = crossHair.GetCrosshairPosition();

        Vector3 dir = crosshairPosition - cameraPosition;
        dir.Normalize();

        RaycastHit hit;
        if (Physics.Raycast(cameraPosition, dir, out hit, 1000.0f))
        {
            if (!hit.collider.isTrigger)
            {
                //Debug.Log("Targeting in 10m: "+hit.collider.gameObject.name);
                if (hit.collider.name == "Enemy2" && firing)
                {
                    //Debug.Log("Enemy2 should be hit!!!" + hit.point);

                }
            }
        }
        bulletTarget = hit.point; //the target we are aiming
        SpawnBullet sb = currentBulletSpawner.GetComponent<SpawnBullet>();
        Debug.DrawLine(cameraPosition, bulletTarget, Color.white);
        if (KinectInput.GetFire() && !firing)
        {
            firing = true;
            GunAnimation.particleSystem.Play();
            //Debug.Log("start firing");
                
            sb.startShooting();
            audio.clip = guneffect;
            audio.Play();

            //Debug.Log("start firing" + bulletTarget);
        }
        if (!KinectInput.GetFire() && firing)
        {
            firing = false; 
            GunAnimation.particleSystem.Stop();

           
            //Debug.Log("stop firing");
            sb.stopShooting();
            audio.Stop();
        }
       

            

        sb.SetSpawnTarget(bulletTarget);



	}




}

