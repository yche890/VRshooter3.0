using UnityEngine;
using System.Collections;
/************************************************************************************

This file describe the behaivour of the player based on the detection of gesutre/physical actions 
from Kinect
Auther:  Yang Chen

************************************************************************************/
public class KinectControlScript : MonoBehaviour {
    public bool changeScene = false;
    private bool shouldChangeRightMost = false;
    public float crouchDeltaHeight = 0.6f;  //crouch height difference
    public float leftRightSpeed;
    public float crouchSpeed;   //how many crouch action per second
    private int state;  //state = stand, crouch etc
    private bool processingCrouch = false;  
    private float crouchDegree = 0.0f;  //crouchDegree between 0 and 1, 0 represent fully standing, 1 represent fully crouching
    private float surrenderDegree = 0.0f;
    private float leftrightDegree = 0.0f;
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
    
    
    private Vector3[] standState = new Vector3[15];
    private Vector3[] crouchState = new Vector3[15];
    
    
    private CharacterController mainCollider;
    private Transform swat,FD,OculusController;
    
    
    Transform lul,rul,ll,rl,lf,rf,ltb,rtb,sp1,nk,gun,bc;
    private Vector3 lulC,rulC,llC,rlC,lfC,rfC,ltbC,rtbC,sp1C,nkC,gunC,ovrC,leftMost,rightMost,swatrightMostPosition,ORrightMostPosition;
    // Use this for initialization
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
        
        //load colliders
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
        if (changeScene && !shouldChangeRightMost)
        {
            // set right most
            
            shouldChangeRightMost = true;
            
            
        }
        
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
            
            /*
             * update transform value of body parts
             */
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
                if (KinectInput.GetLeftRight() != -1 && state == leftStepIn)
                {
                    
                    state = stand;
                }
                if (KinectInput.GetLeftRight() != 1 && state == rightStepIn)
                {
                    
                    state = stand;
                }
                
                if (KinectInput.GetLeftRight() == -1 && state == stand)
                {
                    state = leftStepIn;
                }
                if (KinectInput.GetLeftRight() == 1 && state == stand)
                {
                    state = rightStepIn;
                }
                
                
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
                /*
            swat.localPosition = swat.localPosition + (swatrightMostPosition - swat.localPosition) * leftrightDegree;
            OculusController.localPosition = OculusController.localPosition + (ORrightMostPosition - OculusController.localPosition) * leftrightDegree;
            mainCollider.center = new Vector3(leftrightDegree, -0.35f, 0.0f);
            float angle = -FD.transform.localEulerAngles.y;
            Vector3 r1 = new Vector3()
                Vector3 r2 = new Vector3()
                    Vector3 r3 = new Vector3()

            swat.localPosition = swat.localPosition*-F);
*/
                
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
                
                //Debug.Log("RM"+swatrightMostPosition);
                //Debug.Log("PL"+FD.localPosition);
                //trans.y += 0.01f;
                
                
                //
                //gameObject.transform.localPosition = (rightMost - gameObject.transform.localPosition)*leftrightDegree + gameObject.transform.localPosition;
                
            }
            //        Debug.Log("pos:\t" + gameObject.transform.localPosition);
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
            //        Debug.Log("leftrightDegree"+leftrightDegree);
            
            
            //process pausing
            
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
    public void setRightMost(Vector3 pos){
        
        leftMost = pos;
        rightMost = pos;
        //Debug.Log("initial" + rightMost);
        leftMost.x -= 1;
        rightMost.x += 1;
        
    }
    
    
    
    public void DoCrouch() {
        // decrease the size of collider
        mainCollider.height -= crouchDeltaHeight;
        mainCollider.center -= new Vector3(0,crouchDeltaHeight/2, 0);
        
        state = crouch;
    }
    public void stopCrouching(){
        // resume the size of collider
        state = stand;
        mainCollider.height += crouchDeltaHeight;
        mainCollider.center += new Vector3(0,crouchDeltaHeight/2, 0);
    }
    public static void AddIndexByOne(){
        positionIndex += 1;
        Debug.Log("positionIndex" + positionIndex);
    }
}