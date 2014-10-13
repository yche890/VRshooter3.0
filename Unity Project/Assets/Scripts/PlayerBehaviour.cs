using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class PlayerBehaviour : MonoBehaviour {

    public int setLife = 100;
    public static int life;
    
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
                Time.timeScale = 0;
            }
        }
    }
}
