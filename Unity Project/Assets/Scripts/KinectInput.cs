using UnityEngine;
using System.Collections;
/// <summary>
/// This script use Kinect skeleton wrapper to detect multiple physical actions or gestures.
/// </summary>
public class KinectInput : MonoBehaviour {
    /// <summary>
    /// The oculus rift device, for initializing orientation offset
    /// </summary>
    private OVRDevice device;
    /// <summary>
    /// The dodge restrict.
    /// 1:only left-dodge
    /// 2:only right-dodge
    /// 3: no left or right
    /// 0: no limit
    /// </summary>
    private static int dodgeRestrict;   //1:only left  2:only right 3: no left or right 0: no limit
    /// <summary>
    /// The crouch sensitivity, when player's height is smaller than *crouch sensitivity * player's height) 
    /// We detect player is crouching
    /// </summary>
    public float crouchSensitivity;
    /// <summary>
    /// The sensitivity of firing gesture detection, whether the distance of two hands is smaller than this value, 
    /// is the first condition for detection of player shooting
    /// </summary>
    public float fireSensitivity;
    /// <summary>
    /// The left right sensitivity.
    /// </summary>
    public float leftRightSensitivity;
    /// <summary>
    /// Game is resumed when resume degree == 1.
    /// </summary>
    private float resumeDegree = 0.0f;
    private static bool croch = false;
    /// <summary>
    /// The surrender is currently disabled.
    /// </summary>
    private static bool surrender = false;
    private static bool resume = false;
    private static bool fire = false;
    /// <summary>
    /// The left right offset of player.
    /// left: [-1,0]
    /// right: [0,1]
    /// </summary>
    private static int leftRightState = 0;
    /// <summary>
    /// Whether kinect got players height or not
    /// </summary>
    private static bool gotInitialData = false;
    /// <summary>
    /// The height of the player, and the X position of player in front of kinect
    /// </summary>
    public static float playerHeight = 0.0f, playerCentreX = 0.0f;
    /// <summary>
    /// How much left & right the player moved.
    /// </summary>
    private float LRoffset;
    /// <summary>
    /// The kinect's skeletonwrapper
    /// </summary>
    public SkeletonWrapper sw;  
    /// <summary>
    /// The calibration process between 0 and 1.
    /// When it is 1, calibration is done
    /// </summary>
    private float calibrationProcess;
    /// <summary>
    /// The indexes of points from Kinect's skeleton wrapper.
    /// </summary>
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
    /// <summary>
    /// The position of left feet, right feet and so on.
    /// </summary>
    private Vector3 leftFeet, rightFeet, hip, shoulder, leftHand, rightHand, sp, head; 
    // Use this for initialization
    void Start () {
        var pc = GameObject.Find("OurPlayer/OVRPlayerController");
        device = pc.GetComponentInChildren<OVRDevice>();

    }
    
    // Update is called once per frame
    /// <summary>
    /// Update each point of player's body, and check different gestures
    /// </summary>
    void Update () {


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
        //looking for initial data i.e players height
        //
        if (!gotInitialData)
        {
            if (IsDoingCalibrationAction()){
                calibrationProcess += Time.deltaTime;
            }
            if (calibrationProcess >= 1.0f){
                gotInitialData = true;
                playerHeight = (Vector3.Distance(head, rightFeet) + Vector3.Distance(head, leftFeet)) / 2;
                playerCentreX = CalcPlayerSpineX();
                Debug.Log("player height:" + playerHeight + "\t\tcentre " + playerCentreX);
                leftRightSensitivity = (rightHand.x - leftHand.x) / 2;
                //reset the orientation
                OVRDevice.ResetOrientation();
            }

        }
        else
        {
            // if the kinect is working, spline and hip will not be together
            if (Vector3.Distance(sp, hip) > 0.01)
            {   
                //if the hands are together
                if (Vector3.Distance(leftHand, rightHand) < fireSensitivity)
                {
                    //if the hands are raised 
                    if (leftHand.y > sp.y && rightHand.y > sp.y)
                    {   
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


            /* keyboard crouch press "c" to crouch
             * only for debugging
             *
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
            /* detect crouch
             */
            // if the kinect is working, spline and hip will not be together
            if (Vector3.Distance(shoulder, hip) > 0.05f)
            {   
                // if y value of feet and head/hip is smaller than some value 
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
                // if the kinect is working, spline and hip will not be together
                if (Vector3.Distance(shoulder, hip) > 0.05f)
                {   
                    //detect if both hands are above head
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
                // if the kinect is working, spline and hip will not be together
                if (Vector3.Distance(shoulder, hip) > 0.05f)
                {   
                    //if only right hand is above head
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
    }
    private bool IsDoingCalibrationAction(){
        bool result = false;
        // if height is detected
        if (Vector3.Distance(head, leftFeet) > 0.8 && Vector3.Distance(head, rightFeet) > 0.8)
        {
            // if hands are beside head
            if (head.y - leftHand.y < 0.1 && leftHand.y - rightHand.y < 0.1)
            {
                result = true;
            }
        }
        return result;
    }

    /// <summary>
    /// Sets the restrict of left/right dodge.
    /// </summary>
    /// <param name="restrict">Restrict.</param>
    public void setRestrict(int restrict){
        dodgeRestrict = restrict;
    }
    /// <summary>
    /// Calculates the player' average x coordinate.
    /// </summary>
    /// <returns>The player average x coordinate.</returns>
    public float CalcPlayerSpineX(){
        return (head.x + shoulder.x + sp.x + hip.x) / 4;

    }
    /// <summary>
    /// Gets the crouch.
    /// </summary>
    /// <returns><c>true</c>, if crouching, <c>false</c> otherwise.</returns>
    public static bool GetCrouch(){
        if (leftRightState == 0 && croch)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// Gets the surrender. (currently disabled)
    /// </summary>
    /// <returns><c>true</c>, if surrender was gotten, <c>false</c> otherwise.</returns>
    public static bool GetSurrender(){
            
        //return pause;
        return false;
    }
    /// <summary>
    /// Gets the game resume.
    /// </summary>
    /// <returns><c>true</c>, if resume was gotten, <c>false</c> otherwise.</returns>
    public static bool GetResume(){
        
        //return true;
        return resume;
    }
    /// <summary>
    /// Gets the fire gesture.
    /// </summary>
    /// <returns><c>true</c>, if fire was gotten, <c>false</c> otherwise.</returns>
    public static bool GetFire(){
        
        return fire;
    }
    /// <summary>
    /// Gets the left right state under left/right restriction.
    /// </summary>
    /// <returns>The left right.</returns>
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
    /// <summary>
    /// Gets the left/right dodge restrict.
    /// </summary>
    /// <returns>The left right restrict.</returns>
    public static int getLeftRightRestrict(){
        return dodgeRestrict;
    }
    /// <summary>
    /// Determines if is got initial data.
    /// </summary>
    /// <returns><c>true</c> if is got initial data; otherwise, <c>false</c>.</returns>
    public static bool IsGotInitialData(){
        return gotInitialData;
    }

}
