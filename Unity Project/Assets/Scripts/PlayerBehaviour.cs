using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class PlayerBehaviour : MonoBehaviour {

    public int setLife;
    public static int life;
    private static bool playerDead = false;
    // Use this for initialization
    void Start () {
        life = setLife;
    }
    
    // Update is called once per frame
    void Update () {
    }
    
    void OnCollisionEnter(Collision c){
        if (c.gameObject.tag == "EnemyBullet")
        {
            life = life - 1;
            //Debug.Log("I has been hit, HP - 1");
            if(life == 0){
                playerDead = true;
                Invoke("StopGame", 2.0f);
            }
        }
    }
    void StopGame(){
        Time.timeScale = 0;
    }
    public static bool IsPlayerDead(){
        return playerDead;
    }
}
