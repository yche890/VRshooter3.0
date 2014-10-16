using UnityEngine;
using System.Collections;
/************************************************************************************
/// <summary>
/// This file describe the behaivour of enemies gun
/// Auther: Henry Lee, Yang Chen
/// </summary>
************************************************************************************/
public class EnemyGunScript : MonoBehaviour {
    /// <summary>
    /// The current bullet spawner.
    /// </summary>
    private GameObject currentBulletSpawner;
    private GameObject player;
    private Vector3 playerPosition;
    /// <summary>
    /// The position of gun muzzle
    /// </summary>
    private GameObject self;
    private Vector3 selfPosition;
    private bool firing;
    private GameObject GunAnimation;
    private Vector3 bulletSpawnPos;
    private Vector3 bulletTarget;
    private float rotationSpeed = 3;
    private bool enemyActive = false;
    private float playerheadHeight = 0.0f;
    
    // Use this for initialization
    void Start () {

        firing = false;
        
        var ani = transform.Find("Particle System").gameObject;
        if (ani == null)
        {
            Debug.Log("cant find animation");
        } else
        {
            GunAnimation = ani;
        }
        var bs = transform.Find("BulletSpawn").gameObject;
        if (bs == null)
        {
            Debug.Log("cant find BulletSpawn");
            //initially dont spawn bullet
        } else
        {
            bs.SetActive(false);
            currentBulletSpawner = bs;
        }

        player = GameObject.Find("OVRPlayerController/ForwardDirection/swat/Hips/Spine/Spine1/Spine2/Neck"); ///Neck1/Head/HeadTop_End
        if (player == null)
        {
            Debug.Log("cant find player controller camera");
        } else
        {
            playerPosition = player.transform.position;
            playerheadHeight = player.transform.position.y;

        }

        self = gameObject.transform.parent.gameObject;

    }

    // Update is called once per frame
    void Update () {
        if (!enemyActive)
        {
            transform.parent.gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        } else
        {
            transform.parent.gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;
        }
        playerPosition = player.transform.position;
        playerPosition.y = playerheadHeight;
        selfPosition = currentBulletSpawner.transform.position;
        
        Vector3 dir = playerPosition - selfPosition;
        dir.Normalize();

        var newRotation = Quaternion.Slerp(self.transform.rotation,Quaternion.LookRotation(playerPosition - selfPosition), rotationSpeed*Time.deltaTime).eulerAngles;
        newRotation.x = 0;
        newRotation.z = 0;
        self.transform.rotation = Quaternion.Euler(newRotation);


        RaycastHit hit;
        //Debug.DrawLine(selfPosition, playerPosition, Color.white);
        if (Physics.Raycast(selfPosition, dir, out hit, 1000.0f))

        {
            if (!hit.collider.isTrigger)
            {
                //Debug.Log("Targeting in 10m: "+hit.collider.gameObject.name);

            }
        }
        bulletTarget = playerPosition;//hit.point; //the target we are aiming
        //Debug.Log(bulletTarget);
        EnemySpawnBullet sb = currentBulletSpawner.GetComponent<EnemySpawnBullet>();


        if (enemyActive && !firing)
        {
            firing = true;
            sb.setAnimation(GunAnimation);
            sb.InvokeBullets(2.0f, 3.0f);
        } else if(!enemyActive)
        {
            firing = false;
            sb.stopShooting();
        }


        
        
        
        sb.SetSpawnTarget(bulletTarget);
        
        
        
    }
    public void setEnemyActive(bool ea){
        enemyActive = ea;
    }
    
    
    
}

