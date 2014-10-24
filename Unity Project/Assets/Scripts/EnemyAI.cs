using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy 
Auther:  Henry Lee

************************************************************************************/
public class EnemyAI : MonoBehaviour {
    /// <summary>
    /// The initial life of enemy.
    /// </summary>
    public int setLife = 2;
    /// <summary>
    /// lift counter of enemy
    /// </summary>
    private int life;



	// Use this for initialization, initialize the life
	void Start () {

        life = setLife;
	}
	
	// Update is called once per frame
	void Update () {
        //enemy will not move, otherwise the difficulty is too hard
	}

     /// <summary>
    /// Player hit the enemy
     /// </summary>
     /// <param name="c">collision event.</param>
    void OnCollisionEnter(Collision c){

        if (c.gameObject.tag == "PlayerBullet")
        {
            life = life - 1;
            //Debug.Log(gameObject.name+" has been hit.");
            if(life == 0){
                Destroy(gameObject.transform.parent.gameObject);
                HeadsUpDisplay.enemyKilled();
            }
        }
    }


}
