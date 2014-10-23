using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of the player character based on the detection of gesutre/physical actions 
from Kinect
Auther:  Yang Chen

************************************************************************************/
/// <summary>
/// Kinect control script.
/// </summary>
public class KinectControlScript : MonoBehaviour {
    /// <summary>
    /// Whether the player is moving between different battle positions
    /// </summary>
    public bool changeScene = false;
    /// <summary>
    /// If the new right most position should be assigned
    /// </summary>
    private bool shouldChangeRightMost = false;
    /// <summary>
    /// The height of the collider when crouch
    /// </summary>
    public float crouchDeltaHeight = 0.6f;  //crouch height difference
    /// <summary>
    /// The left right dodge speed, m/s.
    /// </summary>
    public float leftRightSpeed;
    /// <summary>
    /// The crouch speed, m/s.
    /// </summary>
    public float crouchSpeed;
    /// <summary>
    /// The player character's state
    /// 0: stand.
    /// 1: crouch
    /// 2: left-dodge.
    /// 3: right-dodge.
    /// </summary>
    private int state;
    /// <summary>
    /// Whether is processing crouch.
    /// </summary>
    private bool processingCrouch = false;
    /// <summary>
    /// The crouch degree between 0 and 1, 0 represent fully standing, 1 represent fully crouching
    /// </summary>
    private float crouchDegree = 0.0f;
    /// <summary>
    /// The surrender degree, when equals 1, start surrender.
    /// currently disabled
    /// </summary>
    private float surrenderDegree = 0.0f;
    /// <summary>
    /// The leftright offset of the character.
    /// </summary>
    private float leftrightDegree = 0.0f;
    /// <summary>
    /// The index of the battle position.
    /// </summary>
    private static int positionIndex = 0;
    
    
    //states
    private readonly int
        stand = 0,
        crouch = 1,
        leftStepIn = 2,
        rightStepIn = 3;
    //index ot crouch action array
    private readonly int
        LeftULegR = 0,
        LeftLegR = 1,
        RightULegR = 2,
        //RightULegz = 3,
        GunP = 3,
        RightLegR = 4,
        LeftFootR = 5,
        RightFootR = 6,
        LeftToeBaseR = 7,
        RightToeBaseR = 8,
        //upper body
        NeckP = 9,
        SpineP = 10,
        OVRPControlP = 11;
    //Gunx = 12,
    //Guny = 13,
    //Gunz = 14
    
    /// <summary>
    /// When perform crouch, each part of the body need to translate/rotate
    /// The vector array store the information of standing
    /// </summary>
    private Vector3[] standState = new Vector3[15];
    /// <summary>
    /// When perform crouch, each part of the body need to translate/rotate
    /// The vector array store the information of crouching
    /// </summary>
    private Vector3[] crouchState = new Vector3[15];
    
    /// <summary>
    /// The character collider.
    /// </summary>
    private CharacterController mainCollider;
    /// <summary>
    /// The position of swat, forward direction, and oculus rift controller.
    /// </summary>
    private Transform swat,FD,OculusController;
    
    /// <summary>
    /// The Transform of left upper leg, right upper leg, left leg, right leg, left feet, right feet, left toe base, right toe base, spine1, neck, and gun.
    /// </summary>
    Transform lul,rul,ll,rl,lf,rf,ltb,rtb,sp1,nk,gun;
    /// <summary>
    /// The current position/rotation of left upper leg, right upper leg, left leg, right leg, left feet, right feet, left toe base, right toe base, spine1, neck, and gun.
    /// </summary>
    private Vector3 lulC,rulC,llC,rlC,lfC,rfC,ltbC,rtbC,sp1C,nkC,gunC,ovrC,leftMost,rightMost,swatrightMostPosition,ORrightMostPosition;
    // Use this for initialization
    /// <summary>
    /// Find all the gameobject, components, and transforms
    /// </summary>
    void Start () {
        
        state = stand;
        //find all child transforms
        lul = transform.Find("ForwardDirection/swat/Hips/LeftUpLeg");
        rul = transform.Find("ForwardDirection/swat/Hips/RightUpLeg");
        ll = transform.Find("ForwardDirection/swat/Hips/LeftUpLeg/LeftLeg");
        rl = transform.Find("ForwardDirection/swat/Hips/RightUpLeg/RightLeg");
        lf = transform.Find("ForwardDirection/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot");
        rf = transform.Find("ForwardDirection/swat/Hips/RightUpLeg/RightLeg/RightFoot");
        ltb = transform.Find("ForwardDirection/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot/LeftToeBase");
        rtb = transform.Find("ForwardDirection/swat/Hips/RightUpLeg/RightLeg/RightFoot/RightToeBase");
        
        sp1 = transform.Find("ForwardDirection/swat/Hips/Spine/Spine1");
        nk = transform.Find("ForwardDirection/swat/Hips/Spine/Spine1/Spine2/Neck");
        
        gun = transform.Find("ForwardDirection/swat/M4A1 Sopmod");
        swat = transform.Find("ForwardDirection/swat"); // y = -0.2
        FD =  transform.Find("ForwardDirection");
        OculusController = transform.Find("OVRCameraController"); // y = 0
        //load stand position data
        
        standState [LeftULegR] = lul.localEulerAngles;
        standState [RightULegR] = rul.localEulerAngles;
        standState [LeftLegR] = ll.localEulerAngles;
        standState [RightLegR] = rl.localEulerAngles;
        standState [LeftFootR] = lf.localEulerAngles;
        standState [RightFootR] = rf.localEulerAngles;
        standState [LeftToeBaseR] = ltb.localEulerAngles;
        standState [RightToeBaseR] = rtb.localEulerAngles;
        
        standState [SpineP] = sp1.localPosition;
        standState [NeckP] = nk.localPosition;
        standState [GunP] = gun.localPosition;
        
        standState [OVRPControlP] = gameObject.transform.localPosition;
        
        rightMost = transform.parent.position;
        rightMost.x += 1;
        leftMost = transform.parent.position;
        leftMost.x -= 1;
        
        //Debug.Log("main controller: " + standState [OVRPControlP]);
        
        //load crouch position data
        //these values are manually found during experiment
        crouchState = (Vector3[])standState.Clone();
        crouchState [LeftULegR].x = -95 ;
        crouchState [LeftLegR].x = 90;
        crouchState [RightULegR].x = -55 ;
        crouchState [RightULegR].z = 20;
        crouchState [RightLegR].x = 120;
        crouchState [LeftFootR].x = -10 ;
        crouchState [RightFootR].x = -30 ;
        crouchState [LeftToeBaseR].x = -10 ;
        crouchState [RightToeBaseR].x = -30 ;
        
        crouchState [SpineP].y = 0.08f;
        crouchState [NeckP].y = 0.09f;
        crouchState [GunP].x = 0.118f;
        crouchState [GunP].y = 1.2f;
        crouchState [GunP].z = 0.525f;
        
        crouchState [OVRPControlP].y = 1.0f;
        
        //load player collider
        var pc = GameObject.Find("ForwardDirection/swat/Body-swat");
        
        
        mainCollider = gameObject.GetComponent<CharacterController>();
        if (mainCollider == null)
        {
            
            Debug.Log("cant find main collider");
        }
        
        swatrightMostPosition = swat.localPosition;
        swatrightMostPosition.x += 1;
        
        ORrightMostPosition = OculusController.localPosition;
        ORrightMostPosition.x += 1; 
        
    }
    
    // Update is called once per frame
    void Update () {

        changeScene = !SplineController.getStop();
        // if moving to next battle position
        if (changeScene && !shouldChangeRightMost)
        {
            // set right most
            
            shouldChangeRightMost = true;
            
            
        }
        // set rightmost position if arrive a new position
        if (shouldChangeRightMost && !changeScene)
        {
            //Debug.Log("Tryingto dosomething");
            var pickups = GameObject.Find("Pickups").transform;
            if (pickups != null)
            {
                AddIndexByOne();
                string name = "PickUp" + positionIndex.ToString();
                Vector3 pickupPosition = pickups.FindChild(name).position;
                setRightMost(pickupPosition);
                //Debug.Log("process position index is: " + positionIndex);
                shouldChangeRightMost = false;
            } else
            {
                Debug.Log("failed with finding:");
            }
        }
        // perform crouch, left right dodge 
        if (!changeScene)
        {
            if (!KinectInput.GetCrouch() && state == crouch)
            {
                state = stand;
                
                
                stopCrouching();
                
            }
            
            if (KinectInput.GetCrouch() && state == stand)
            {
                state = crouch;
                
                DoCrouch();
                
                
            } 
            
            if (state == crouch)
            {
                //increate crouch degree
                crouchDegree += Time.deltaTime * crouchSpeed;
                if (crouchDegree > 1)
                {
                    crouchDegree = 1;
                }
            }
            if (state == stand)
            {
                //decrease crouch degree
                crouchDegree -= Time.deltaTime * crouchSpeed;
                if (crouchDegree < 0)
                {
                    crouchDegree = 0;
                }
                
                
                
            }
            if (crouchDegree != 0)
            {
                processingCrouch = true;
            } else
            {
                processingCrouch = false;
            }
            
            /**************************************************************************************
             * update position and rotation value of each body parts with respect to crouch degree...
             **************************************************************************************/

            //LeftUpLeg
            lulC = Vector3.Lerp(standState [LeftULegR], crouchState [LeftULegR], crouchDegree);
            
            lulC.x = lulC.x < 0 ? lulC.x + 360 : lulC.x;
            //Debug.Log("lulC.x: " + standState [LeftULegR].x);
            //Debug.Log("crouch:"+crouchDegree);
            lul.transform.localRotation = Quaternion.Euler(lulC);
            
            //RightUpLeg
            rulC = Vector3.Lerp(standState [RightULegR], crouchState [RightULegR], crouchDegree);
            rulC.x = rulC.x < 0 ? rulC.x + 360 : rulC.x;
            rul.transform.localRotation = Quaternion.Euler(rulC);
            //LeftLeg
            llC = Vector3.Lerp(standState [LeftLegR], crouchState [LeftLegR], crouchDegree);
            llC.x = llC.x < 0 ? llC.x + 360 : llC.x;
            ll.transform.localRotation = Quaternion.Euler(llC);
            //RightLeg
            rlC = Vector3.Lerp(standState [RightLegR], crouchState [RightLegR], crouchDegree);
            rlC.x = rlC.x < 0 ? rlC.x + 360 : rlC.x;
            rl.transform.localRotation = Quaternion.Euler(rlC);
            //rl.transform.localRotation = Quaternion.Euler(crouchState [RightLegR]);
            //LeftFeet
            lfC = Vector3.Lerp(standState [LeftFootR], crouchState [LeftFootR], crouchDegree);
            lfC.x = lfC.x < 0 ? lfC.x + 360 : lfC.x;
            lf.transform.localRotation = Quaternion.Euler(lfC);
            //lf.transform.localRotation = Quaternion.Euler(crouchState [LeftFootR]);
            //RightFeet
            rfC = Vector3.Lerp(standState [RightFootR], crouchState [RightFootR], crouchDegree);
            rfC.x = rfC.x < 0 ? rfC.x + 360 : rfC.x;
            rf.transform.localRotation = Quaternion.Euler(rfC);
            //rf.transform.localRotation = Quaternion.Euler(crouchState [RightFootR]);
            //LeftToeBase
            ltbC = Vector3.Lerp(standState [LeftToeBaseR], crouchState [LeftToeBaseR], crouchDegree);
            ltbC.x = ltbC.x < 0 ? ltbC.x + 360 : ltbC.x;
            ltb.transform.localRotation = Quaternion.Euler(ltbC);
            //ltb.transform.localRotation = Quaternion.Euler(crouchState [LeftToeBaseR]);
            //RightToeBase
            rtbC = Vector3.Lerp(standState [RightToeBaseR], crouchState [RightToeBaseR], crouchDegree);
            rtbC.x = rtbC.x < 0 ? rtbC.x + 360 : rtbC.x;
            rtb.transform.localRotation = Quaternion.Euler(rtbC);
            //rtb.transform.localRotation = Quaternion.Euler(crouchState [RightToeBaseR]);
            //Spine1
            sp1C = Vector3.Lerp(standState [SpineP], crouchState [SpineP], crouchDegree);
            sp1.transform.localPosition = sp1C;
            
            //Neck
            nkC = Vector3.Lerp(standState [NeckP], crouchState [NeckP], crouchDegree);
            nk.transform.localPosition = nkC;
            //Gun
            gunC = Vector3.Lerp(standState [GunP], crouchState [GunP], crouchDegree);
            gun.transform.localPosition = gunC;
            // smoothly change position 
            OculusController.localPosition = new Vector3(0, - crouchDeltaHeight * crouchDegree, 0);
            swat.localPosition = new Vector3(0, -0.2f - crouchDeltaHeight * crouchDegree, 0);
            
            //if not crouching
            
            if (!processingCrouch && !changeScene)
            {
                // if kinect detect right, and state of character is left
                if (KinectInput.GetLeftRight() != -1 && state == leftStepIn)
                {
                    
                    state = stand;
                }
                // if kinect detect left, and state of character is right
                if (KinectInput.GetLeftRight() != 1 && state == rightStepIn)
                {
                    
                    state = stand;
                }
                // if kinect detect left, and state of character is standing
                if (KinectInput.GetLeftRight() == -1 && state == stand)
                {
                    state = leftStepIn;
                }
                // if kinect detect right, and state of character is standing
                if (KinectInput.GetLeftRight() == 1 && state == stand)
                {
                    state = rightStepIn;
                }
                
                /**********************************************************
                 * updating the leftrightDegree...........................
                 * ********************************************************/
                if (state == leftStepIn)
                {
                    //increate leftrightDegree
                    leftrightDegree -= Time.deltaTime * leftRightSpeed;
                    if (leftrightDegree < -1)
                    {
                        leftrightDegree = -1;
                    }
                    
                }
                if (state == rightStepIn)
                {
                    //increate leftrightDegree
                    leftrightDegree += Time.deltaTime * leftRightSpeed;
                    if (leftrightDegree > 1)
                    {
                        leftrightDegree = 1;
                    }
                }
                if (state == stand)
                {
                    if (leftrightDegree > 0)
                    {
                        leftrightDegree -= Time.deltaTime * leftRightSpeed;
                        if (leftrightDegree < 0)
                        {
                            leftrightDegree = 0;
                        }
                    }
                    if (leftrightDegree < 0)
                    {
                        leftrightDegree += Time.deltaTime * leftRightSpeed;
                        if (leftrightDegree > 0)
                        {
                            leftrightDegree = 0;
                        }
                    }
                    
                }
           
             /**************************************************************************************
             * update position value of player with respect to leftright degree...
             **************************************************************************************/
                Vector3 positioning = rightMost;
                positioning.x = positioning.x - leftrightDegree - 1;
                
                if (positioning.x > rightMost.x)
                {
                    positioning.x = rightMost.x;
                }
                if (positioning.x < leftMost.x)
                {
                    positioning.x = leftMost.x;
                }
                Vector3 trans = positioning - transform.parent.position;
                trans.y = 0.0f;
                transform.parent.Translate(trans);    
                
               
                
            }

            switch (state)
            {
                case 0:
                    Debug.Log("state: stand");
                    break;
                case 1:
                    Debug.Log("state: crouch");
                    break;
                case 2:
                    Debug.Log("state: leftStepIn");
                    break;
                case 3:
                    Debug.Log("state: rightStepIn");
                    break;
            }

            
            
            //process pausing
            //currently disabled
            
            if (KinectInput.GetSurrender() == true)
            {
                //increate surrender degree
                surrenderDegree += Time.deltaTime;
                if (surrenderDegree > 1)
                {
                    surrenderDegree = 1;
                }
            } else
            {
                //decrease surrender degree
                surrenderDegree -= Time.deltaTime;
                if (surrenderDegree < 0)
                {
                    surrenderDegree = 0;
                }
            }
            
            if (surrenderDegree == 1 && Time.timeScale != 0)
            {
                Time.timeScale = 0;
                
            }
            if (KinectInput.GetResume())
            {
                Time.timeScale = 1.0f;
            }
            
        }
    }
    /// <summary>
    /// Sets the right most position when arrive a new battle position.
    /// </summary>
    /// <param name="pos">Position.</param>
    public void setRightMost(Vector3 pos){
        
        leftMost = pos;
        rightMost = pos;
        //Debug.Log("initial" + rightMost);
        leftMost.x -= 1;
        rightMost.x += 1;
        
    }

    /// <summary>
    /// // decrease the size of collider immediately
    /// </summary>
    public void DoCrouch() {

        mainCollider.height -= crouchDeltaHeight;
        mainCollider.center -= new Vector3(0,crouchDeltaHeight/2, 0);
        state = crouch;
    }
    /// <summary>
    /// Resume the size of collider.
    /// </summary>
    public void stopCrouching(){
        // resume the size of collider
        state = stand;
        mainCollider.height += crouchDeltaHeight;
        mainCollider.center += new Vector3(0,crouchDeltaHeight/2, 0);
    }
    /// <summary>
    /// Increment the index of battle position
    /// </summary>
    public static void AddIndexByOne(){
        positionIndex += 1;
        Debug.Log("positionIndex" + positionIndex);
    }
}