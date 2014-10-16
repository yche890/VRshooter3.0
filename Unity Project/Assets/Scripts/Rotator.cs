using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the rorating pickup trigger
Auther:  Henry Lee

************************************************************************************/
public class Rotator : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.right * Time.deltaTime * 50);
	}
}
