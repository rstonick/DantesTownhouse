using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class RootMotionControlScript : MonoBehaviour
{

    [SerializeField]
    private float jumpRegTime;
    private Animator anim;	
    private Rigidbody rbody;
    private CharacterInputController cinput;
    private CharacterController characterController;

    private Transform leftFoot;
    private Transform rightFoot;


    public GameObject buttonPressStandingSpot;
    public GameObject buttonObject;
    public float buttonCloseEnoughForMatchDistance = 2f;
    public float buttonCloseEnoughForPressDistance = 0.22f;
    public float buttonCloseEnoughForPressAngleDegrees = 5f;
    public float initalMatchTargetsAnimTime = 0.25f;
    public float exitMatchTargetsAnimTime = 0.75f;
    public float animationSpeed = 1f;
    public float rootMovementSpeed = 1f;
    public float rootTurnSpeed = 1f;
    public float jumpForce = 200f;
    private float originalStepOffset;
   


    // classic input system only polls in Update()
    // so must treat input events like discrete button presses as
    // "triggered" until consumed by FixedUpdate()...
    bool _inputActionFired = false;
    bool _inputJumpFired = false;
    bool _jumping = false;
    bool _jumpingGrounded = false;

    private float _gravityValue = 9.81f;
    private float lastGroundedTime;
    private float jumpButtonPressedTime;
    private float ySpeed;

    // ...however constant input measures like axes can just have most recent value
    // cached.
    float _inputForward = 0f;
    float _inputTurn = 0f;


    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;


    private int groundContactCount = 0;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");
    }


    // Use this for initialization
    void Start()
    {
		//example of how to get access to certain limbs
        leftFoot = this.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
        rightFoot = this.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot");
        if (leftFoot == null || rightFoot == null)
            Debug.Log("One of the feet could not be found");
            
    }


    private void Update()
    {
        if (cinput.enabled)
        {
            _inputForward = cinput.Forward;
            _inputTurn = cinput.Turn;

            // Note that we don't overwrite a true value already stored
            // Is only cleared to false in FixedUpdate()
            // This makes certain that the action is handled!
            _inputActionFired = _inputActionFired || cinput.Action;
            _inputJumpFired = _inputJumpFired || cinput.Jump;
            bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

            if (!anim.applyRootMotion)
            {
                float inputForward = 0f;
                float inputTurn = 0f;

                if (cinput.enabled)
                {
                    inputForward = cinput.Forward;
                    inputTurn = cinput.Turn;
                }

                //switch turn around if going backwards

                rbody.MovePosition(rbody.position + transform.forward * inputForward * Time.deltaTime * 5f);
                rbody.MovePosition(rbody.position + transform.right * inputTurn * Time.deltaTime * 8f);
                rbody.MoveRotation(rbody.rotation * Quaternion.AngleAxis(inputTurn * Time.deltaTime * 150f, Vector3.up));
            }

        }

        ySpeed += Physics.gravity.y * Time.deltaTime;
    }

    void FixedUpdate()
    {

        bool doButtonPress = false;
        bool doMatchToButtonPress = false;
        Vector3 movementDirection = new Vector3(_inputTurn, 0, _inputForward);
        //onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground.
        //This is good for allowing player to jump and not be frustrated that the jump button doesn't
        //work
        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        float buttonDistance = float.MaxValue;
        float buttonAngleDegrees = float.MaxValue;
        anim.speed = animationSpeed;
        if (buttonPressStandingSpot != null)
        {
            buttonDistance = Vector3.Distance(transform.position, buttonPressStandingSpot.transform.position);
            buttonAngleDegrees = Quaternion.Angle(transform.rotation, buttonPressStandingSpot.transform.rotation);
        }

        if(_inputActionFired)
        {
            _inputActionFired = false; // clear the input event that came from Update()

            Debug.Log("Action pressed");

            if (buttonDistance <= buttonCloseEnoughForMatchDistance)
            {
                if(buttonDistance <= buttonCloseEnoughForPressDistance &&
                    buttonAngleDegrees <= buttonCloseEnoughForPressAngleDegrees)
                {
                    Debug.Log("Button press initiated");

                    doButtonPress = true;
                    
                }
                else
                {
                    // TODO UNCOMMENT THESE LINES FOR TARGET MATCHING
                    Debug.Log("match to button initiated");
                    doMatchToButtonPress = true;
                }

            }
        }
        Debug.Log(anim.applyRootMotion);



        if (isGrounded)
        {
            Debug.Log(groundContactCount);
            ySpeed = -0.5f;
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            _jumping = false;
            anim.SetBool("JumpPressed", false);
            anim.SetBool("isFalling", false);
            
            if (_inputJumpFired)
            {
                ySpeed = jumpForce;
                Debug.Log(transform.forward);
                Vector3 velocity = anim.deltaPosition;
                velocity.y = ySpeed * Time.deltaTime;

                //rbody.velocity = new Vector3(0,10,0);

                rbody.AddForce(new Vector3(0, 5, 0) * jumpForce, ForceMode.Impulse);
                _jumping = true;
                anim.SetBool("JumpPressed", true);
                
                _inputJumpFired = false;
                _jumping = true;

            }

        }
        else
        {
            anim.SetBool("isGrounded", false);
            isGrounded = false;

            if ((_jumping && ySpeed < 0) || ySpeed < -2)
            {
                anim.SetBool("isFalling", true);
            }
        }

        // get info about current animation
        //var animState = anim.GetCurrentAnimatorStateInfo(0);

        //// If the transition to button press has been initiated then we want
        //// to correct the character position to the correct place

        //if (animState.IsName("MatchToButtonPress")
        //    && !anim.IsInTransition(0) && !anim.isMatchingTarget)
        //{
        //    if (buttonPressStandingSpot != null)
        //    {
        //        Debug.Log("Target matching correction started");

        //        initalMatchTargetsAnimTime = animState.normalizedTime;

        //        var t = buttonPressStandingSpot.transform;
        //        anim.MatchTarget(t.position, t.rotation, AvatarTarget.Root,
        //            new MatchTargetWeightMask(new Vector3(1f, 0f, 1f),
        //            1f),
        //            initalMatchTargetsAnimTime,
        //            exitMatchTargetsAnimTime);
        //    }
        //}

        if (movementDirection != Vector3.zero)
        {
            
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
            
        }

        if (!isGrounded) anim.applyRootMotion = false;
        else anim.applyRootMotion = true;
        Debug.Log(anim.applyRootMotion);
        //if (rbody.velocity.y < 0)
        //{
        //    Debug.Log("velocity1");
        //    Debug.Log(anim.applyRootMotion);
        //    rbody.velocity += Vector3.up * Physics.gravity.y * (250f - 1) * Time.deltaTime;
        //}
        //else if (rbody.velocity.y > 0 && !_inputJumpFired)
        //{
        //    Debug.Log("velocity2");
        //    Debug.Log(anim.applyRootMotion);
        //    rbody.velocity += Vector3.up * Physics.gravity.y * (200f - 1) * Time.deltaTime;
        //}
        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);
        
        anim.SetBool("doButtonPress", doButtonPress);
        anim.SetBool("matchToButtonPress", doMatchToButtonPress);

    }


    //This is a physics callback
    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {

            ++groundContactCount;

            // Generate an event that might play a sound, generate a particle effect, etc.
            EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);

        }
						
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
            --groundContactCount;
        }

    }


    private void OnAnimatorIK(int layerIndex)
    {
        if(anim)
        {
            AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo(0);

            if(astate.IsName("ButtonPress"))
            {
                float buttonWeight = anim.GetFloat("buttonClose");

                // Set the look target position, if one has been assigned
                if(buttonObject != null)
                {
                    anim.SetLookAtWeight(buttonWeight);
                    anim.SetLookAtPosition(buttonObject.transform.position);
                    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, buttonWeight);
                    anim.SetIKPosition(AvatarIKGoal.RightHand,
                        buttonObject.transform.position);
                }
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetLookAtWeight(0);
            }
        }
    }




}
