using UnityEngine;
using System.Collections;
using OVR;

/// <summary>
/// Player's M4A1 gun script.
/// </summary>
public class GunScript : MonoBehaviour {
    /// <summary>
    /// The current bullet spawner.
    /// </summary>
    private GameObject currentBulletSpawner;
    /// <summary>
    /// The camera position, used for aiming.
    /// </summary>
    private Vector3     cameraPosition;
    /// <summary>
    /// The crosshair position.
    /// </summary>
    private Vector3     crosshairPosition;
    /// <summary>
    /// The oculus rift's main menu, get crosshair from it.
    /// </summary>
    private OVRMainMenu mm;
    /// <summary>
    /// The oculus rift's camera controller, get interpupillary distance(IPD)
    /// </summary>
    private OVRCameraController occ;
    /// <summary>
    /// The cross hair.
    /// </summary>
    private OVRCrosshair crossHair;
    /// <summary>
    /// The firing state.
    /// </summary>
    private bool firing;
    /// <summary>
    /// The gun animation.
    /// </summary>
    private GameObject GunAnimation;
    /// <summary>
    /// The bullet spawner position.
    /// </summary>
    private Vector3 bulletSpawnPos;
    /// <summary>
    /// The bullet target.
    /// </summary>
    private Vector3 bulletTarget;
    /// <summary>
    ///1: right eye to aim,
    ///-1 left eye to aim;
    ///0: both eyes
    /// </summary>
    public int usingWhichEyeToAim;
    /// <summary>
    /// The guneffect audio.
    /// </summary>
    public AudioClip guneffect;


	// Use this for initialization, get components
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
	
	/// <summary>
    /// We get the position of crosshair, and the camera of player, draw a ray from camera through the crosshair, and the target will be there
    /// </summary>
	void Update () {


        crossHair = mm.GetCrosshair();
        cameraPosition = crossHair.GetCameraPosition();
        float ipd = occ.IPD;
        cameraPosition.x -= ipd * usingWhichEyeToAim;

        crosshairPosition = crossHair.GetCrosshairPosition();

        Vector3 dir = crosshairPosition - cameraPosition;
        dir.Normalize();

        RaycastHit hit;
        /// testing
        if (!Physics.Raycast(cameraPosition, dir, out hit, 1000.0f))
        {
            Debug.LogError("no target on the crosshair!");
            bulletTarget = crosshairPosition;
        }
        bulletTarget = hit.point; //the target we are aiming
        SpawnBullet sb = currentBulletSpawner.GetComponent<SpawnBullet>();
        Debug.DrawLine(cameraPosition, bulletTarget, Color.white);
        //get firing from Kinectinput
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
       

            
        //set the bullet target
        sb.SetSpawnTarget(bulletTarget);



	}




}

