using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of player's body when hit by enemy
Auther:  Kevin Du

************************************************************************************/
/// <summary>
/// Auther: Kevin Du
/// Date: 08/2014
/// Function: Created effects of police car lights rotation
/// </summary>
public class SirenEffect : MonoBehaviour {

	[SerializeField] Light redLight;
	[SerializeField] Light blueLight;
	[SerializeField] int speed;

	private Vector3 redTemp;
	private Vector3 blueTemp;
	// Update is called once per frame
    // calculate lights rotation
	void Update () {
        // calculate lights rotation
		redTemp.y += speed * Time.deltaTime;
		blueTemp.y -= speed * Time.deltaTime;

		redLight.transform.eulerAngles = redTemp;
		blueLight.transform.eulerAngles = blueTemp;
	}
}
