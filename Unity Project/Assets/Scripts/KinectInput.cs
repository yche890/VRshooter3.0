using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of an enemy bullet
Auther:  Yang Chen

************************************************************************************/
public class KinectInput : MonoBehaviour {
    private static int dodgeRestrict;   //1:only left  2:only right 3: no left or right 0: no limit

    public float crouchSensitivity;
    public float fireSensitivity;
    public float leftRightSensitivity;
    private float resumeDegree = 0.0f;
    private static bool croch = false;
    private static bool surrender = false;
    private static bool resume = false;
    private static bool fire = false;
    private static int leftRightState = 0;
    private bool gotInitialData = false;    //if kinect got players data, i.e height of the player
    public static float playerHeight = 0.0f, playerCentreX = 0.0f;    //store the height of player and the X position of player
    private float LRoffset; //store how much left & right the player moved.
    public SkeletonWrapper sw;  //kinect's skeletonwrapper
    //indexes of points from sw
    private int HipCenter = 0,
    Spine = 1,
    ShoulderCenter = 2,
    Head = 3,
    ShoulderLeft = 4,
    ElbowLeft = 5,
    WristLeft = 6,
    HandLeft = 7,
    ShoulderRight = 8,
    ElbowRight = 9,
    WristRight = 10,
    HandRight = 11,
    HipLeft = 12,
    KneeLeft = 13,
    AnkleLeft = 14,
    FootLeft = 15,
    HipRight = 16,
    KneeRight = 17,
    AnkleRight = 18,
    FootRight = 19;
    private Vector3 leftFeet, rightFeet, hip, shoulder, leftHand, rightHand, sp, head; 
    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        //getting gun fire gesture

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

            leftFeet.x = sw.bonePos [0, AnkleLeft].x;
            leftFeet.y = sw.bonePos [0, AnkleLeft].y;
            leftFeet.z = sw.bonePos [0, AnkleLeft].z;
            
            rightFeet.x = sw.bonePos [0, AnkleRight].x;
            rightFeet.y = sw.bonePos [0, AnkleRight].y;
            rightFeet.z = sw.bonePos [0, AnkleRight].z;
            
            shoulder.x = sw.bonePos [0, ShoulderCenter].x;
            shoulder.y = sw.bonePos [0, ShoulderCenter].y;
            shoulder.z = sw.bonePos [0, ShoulderCenter].z;

            hip.x = sw.bonePos [0, HipCenter].x;
            hip.y = sw.bonePos [0, HipCenter].y;
            hip.z = sw.bonePos [0, HipCenter].z;

            head.x = sw.bonePos [0, Head].x;
            head.y = sw.bonePos [0, Head].y;
            head.z = sw.bonePos [0, Head].z;

        }
        if (Vector3.Distance(head, rightFeet) > 1.0 && Vector3.Distance(head, leftFeet) > 1.0 && !gotInitialData)
        {
            gotInitialData = true;
            playerHeight = (Vector3.Distance(head, rightFeet) + Vector3.Distance(head, leftFeet)) / 2;
            playerCentreX = CalcPlayerSpineX();
            Debug.Log("player height:" + playerHeight + "\t\tcentre " + playerCentreX);
        }

        // if the kinect is working
        if (Vector3.Distance(sp, hip) > 0.01)
        {   
            if (Vector3.Distance(leftHand, rightHand) < fireSensitivity)    //if the hands are together
            {
                if (leftHand.y > sp.y && rightHand.y > sp.y)
                {   //if the hands are raised 
                    fire = true;
                } else
                {
                    fire = false;
                }
            } else
            {
                fire = false;
            }
            
        } else
        {
            if (Input.GetButtonDown("Fire1") && !fire)
            {
                fire = true;
                
                
                //Debug.Log("start firing" + bulletTarget);
            }
            if (Input.GetButtonUp("Fire1") && fire)
            {
                fire = false; 
                
            }
        }


        /* keyboard crouch
        // get crouch gesture 
        if (Input.GetButtonDown("Crouch") && !croch)
        {
            croch = true;
            
            
            //Debug.Log("start firing" + bulletTarget);
        }
        if (Input.GetButtonUp("Crouch") && croch)
        {
            croch = false; 
            
        }*/

        
        //Debug.Log(hip.y-leftFeet.y);

        if (Vector3.Distance(shoulder, hip) > 0.05f)
        {   

            if (hip.y - leftFeet.y < crouchSensitivity * playerHeight * 0.6 || hip.y - rightFeet.y < crouchSensitivity * playerHeight * 0.6)
            {

                croch = true;

            } else
            {
                croch = false;
            }
            
        } else
        {
            croch = false;
        }
        // detect left right movement
        LRoffset = CalcPlayerSpineX() - playerCentreX;
        //Debug.Log("LRoffset: " + LRoffset);
        if (LRoffset < -leftRightSensitivity)
        {   
            leftRightState = -1;
                
        } else if (LRoffset > leftRightSensitivity)
        {
            leftRightState = 1;
        } else
        {
            leftRightState = 0;
        }

        if (croch == false)
        {
            if (Vector3.Distance(shoulder, hip) > 0.05f)
            {   
                //Debug.Log("dis:" + (leftHand.y - head.y));
                if (leftHand.y > head.y && rightHand.y > head.y)
                {
                    
                    surrender = true;
                    
                } else
                {
                    surrender = false;
                }
                
            } else
            {
                surrender = false;
            }

        }
        if (croch == false)
        {
            if (Vector3.Distance(shoulder, hip) > 0.05f)
            {   
                //Debug.Log("dis:" + (leftHand.y - head.y));
                if (leftHand.y < head.y && rightHand.y > head.y)
                {
                    //Debug.Log("resumeDegree++");
                    resumeDegree += 0.1f;
                    if (resumeDegree > 1)
                    {
                        resumeDegree = 1;
                    }
                    
                } else
                {
                    resumeDegree -= 0.1f;
                    if (resumeDegree < 0)
                    {
                        resumeDegree = 0;
                    }
                }
                
            } else
            {
                resumeDegree -= Time.deltaTime;
                if (resumeDegree < 0)
                {
                    resumeDegree = 0;
                }
            }
            //Debug.Log("resumeDegree" + resumeDegree);

        }
        if (resumeDegree == 1)
        {
            resume = true;
         
        } else
        {
            resume = false;
        }
    
    

    }
    public void setRestrict(int x){
        dodgeRestrict = x;
    }
    public float CalcPlayerSpineX(){
        return (head.x + shoulder.x + sp.x + hip.x) / 4;

    }
    public static bool GetCrouch(){
        if (leftRightState == 0 && croch)
        {

            return true;
        }
        return false;
    }

    public static bool GetSurrender(){
            
        //return pause;
        return false;
    }
    public static bool GetResume(){
        
        //return true;
        return resume;
    }

    public static bool GetFire(){
        
        return fire;
    }
    public static int GetLeftRight(){
        if (GetCrouch() == false && dodgeRestrict != 2 && leftRightState == -1 && dodgeRestrict != 3)
        {    //return left dodge

            return -1;
        }
        if (GetCrouch() == false && dodgeRestrict != 1 && leftRightState == 1 && dodgeRestrict != 3)
        {    //return right dodge
            return 1;
        }


        //return leftRightState;
        return 0;
    }
    public static int getLeftRightRestrict(){
        return dodgeRestrict;
    }

}
