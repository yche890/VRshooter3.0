using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Henry Lee

************************************************************************************/
public class Rotator : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.right * Time.deltaTime * 50);
	}
}
