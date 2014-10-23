using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of player's body when hit by enemy
Auther:  Yang Chen, Henry Lee

************************************************************************************/
public class PlayerBehaviour : MonoBehaviour {
    /// <summary>
    /// The initial health point.
    /// </summary>
    public int setLife;
    /// <summary>
    /// The currentlife.
    /// </summary>
    public static int life;
    /// <summary>
    /// The player is dead or not.
    /// </summary>
    private static bool playerDead = false;
    // Use this for initialization
    void Start () {
        life = setLife;
    }
    
    // Update is called once per frame
    void Update () {
    }
    /// <summary>
    /// Player is hit by enemy bullet, and stop the game when player is dead
    /// </summary>
    /// <param name="c">collision</param>
    void OnCollisionEnter(Collision c){
        if (c.gameObject.tag == "EnemyBullet")
        {
            life = life - 1;
            //Debug.Log("I has been hit, HP - 1");
            if(life == 0){
                playerDead = true;
                Invoke("StopGame", 1.0f);
            }
        }
    }
    /// <summary>
    /// Stops the game.
    /// </summary>
    void StopGame(){
        Time.timeScale = 0;
    }
    /// <summary>
    /// Determines if is player dead.
    /// </summary>
    /// <returns><c>true</c> if player is dead; otherwise, <c>false</c>.</returns>
    public static bool IsPlayerDead(){
        return playerDead;
    }
}
