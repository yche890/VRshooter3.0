using UnityEngine;
using System.Collections;
/************************************************************************************

This file is used for testing? 
Auther:  Yang Chen

************************************************************************************/
public class FireControl : MonoBehaviour {

    private static bool fire = false;
    public SkeletonWrapper sw;  //used for firing
    private int HandLeft = 7, HandRight = 11, HipCenter = 0, Spine = 1;
    private Vector3 leftHand, rightHand, hip, sp; 
    // Use this for initialization
    void Start () {

    }
    
    // Update is called once per frame
    void Update () {

        if (Input.GetButtonDown("Fire1") && !fire)
        {
            fire = true;
           
            
            //Debug.Log("start firing" + bulletTarget);
        }
        if (Input.GetButtonUp("Fire1") && fire)
        {
            fire = false; 

        }
        if (sw.pollSkeleton())
        {
            leftHand.x = sw.bonePos [0, HandLeft].x;
            leftHand.y = sw.bonePos [0, HandLeft].y;
            leftHand.z = sw.bonePos [0, HandLeft].z;

            rightHand.x = sw.bonePos [0, HandRight].x;
            rightHand.y = sw.bonePos [0, HandRight].y;
            rightHand.z = sw.bonePos [0, HandRight].z;

            sp.x = sw.bonePos [0, Spine].x;
            sp.y = sw.bonePos [0, Spine].y;
            sp.z = sw.bonePos [0, Spine].z;
            hip.x = sw.bonePos [0, HipCenter].x;
            hip.y = sw.bonePos [0, HipCenter].y;
            hip.z = sw.bonePos [0, HipCenter].z;
        }

        if (Vector3.Distance(sp, hip) > 0.01)
        {
            if (Vector3.Distance(leftHand, rightHand) < 0.2)
            {
                if (leftHand.y > sp.y && rightHand.y > sp.y){
                    fire = true;
                }else{
                    fire = false;
                }
            }else{
                fire = false;
            }

        } else
        {
            fire = false;
        }
    



    }
    
    public static bool GetFire(){
        
        return fire;
    }

}
