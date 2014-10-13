using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class EnemyAI : MonoBehaviour {

    public int setLife = 1;
    private int life;



	// Use this for initialization
	void Start () {
        if (setLife == 7)
        {
            life = 2;
        }else if (setLife < 10)
        {
            life = 3;
        } else
        {
            life = setLife;
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

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
