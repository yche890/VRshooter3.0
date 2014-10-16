using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class EnemyAI : MonoBehaviour {

    public int setLife = 3;
    private int life;



	// Use this for initialization
	void Start () {

        life = setLife;
	}
	
	// Update is called once per frame
	void Update () {
        //enemy will not move, otherwise the difficulty is too hard
	}
    /*
     * Player hit the enemy
     */
    void OnCollisionEnter(Collision c){
        Debug.Log("player collider:" + c.ToString());
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
