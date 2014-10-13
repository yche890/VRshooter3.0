using UnityEngine;
using System.Collections;

public class LightFlash : MonoBehaviour {
	private GameObject leftlight3;
	private GameObject shapeLeftlight3;
	private int offTimes = 0;
	private int onTimes = 0;

	void Start(){
		// Get Leftlight3 object
		// Get shapeOfLeftlight3 object
		leftlight3 = GameObject.Find ("Left light 3");
		shapeLeftlight3 = GameObject.Find ("Left light 3/Shape left light 3");

		// Check for error
		if(leftlight3 == null){
			Debug.Log ("Left light 3 can't be found." );
		}
		if(shapeLeftlight3 == null){
			Debug.Log ("Left light 3/Shape left light 3 can't be found." );
		}
	}
	// Update is called once per frame
	void Update () {

		if (offTimes == 0 && onTimes == 0) {

			// Reset times for light on and off if they are both 0.
			offTimes = Random.Range (5, 50);
			onTimes = Random.Range (5, 50);

			// Calculate light on and off time and operate light on and off.
			OperateLight(leftlight3,shapeLeftlight3);
		} 
		else {
			OperateLight(leftlight3,shapeLeftlight3);
		}
	}

	//Calculate light on and off time and operate light on and off.
	void OperateLight(GameObject light, GameObject shape)
	{
		// Check if both time out just return
		if (onTimes == 0 && offTimes == 0)
			return;

		if (onTimes > 0) {
			// decrease light-on time and keep light on
			onTimes--;
			light.GetComponent<Light> ().enabled = true;
			shape.GetComponent<MeshRenderer> ().enabled = true;
		} 
		else {
			// decrease light-off time and keep light off
			offTimes--;
			light.GetComponent<Light> ().enabled = false;
			shape.GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
