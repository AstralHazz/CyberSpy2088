using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 12.5f, runSpeed = 13;
    public CharacterController myController;
    public float mouseSensitivity = 100f;
    public Transform myCameraHead;
    public Animator myAnimator;
    private float cameraVerticalRotation;
    public float sprintSpeed;

    //jumping section
    private bool readyToJump;
    public float jumpHeight = 1f;
    public Transform ground;
    public LayerMask groundLayer;
    public float groundDist = 0.5f;

    // Adding Gravity
    public Vector3 velocity;
    public float gravityModifier;

    //Crouching
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 bodyScale;
    public Transform myBody;
    private float initialControllerHeight;
    public float crouchSpeed = 6f;
    private bool isCrouching = false;

    //sliding
    private bool isRunning = false;
    private bool startSliderTimer;
    public float currentSlideTimer, maxSlideTime = 2f;
    public float slideSpeed = 10f;

    //hookshot
    public Transform hitPointTransform;
    private Vector3 hookShotPosition;
    public float hookShotSpeed = 5f;
    private Vector3 flyingCharacterMomentum;

    //player states
    private State state;
    private enum State { Normal, HookFlyingPlayer }

    // Start is called before the first frame update
    void Start()
    {
        bodyScale = myBody.localScale;
        initialControllerHeight = myController.height;

        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                PlayerMovement();
                CameraMovement();
                Jump();
                Crouching();
                SlideCounter();
                HandleHookShotStart();
                break;

            case State.HookFlyingPlayer:
                CameraMovement();
                HandleHookShotMovement();
                break;

            default:
                break;
        }
    }

    private void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
            StartCrouching();
        if (Input.GetKeyUp(KeyCode.C) || currentSlideTimer > maxSlideTime)
            StopCrouching();
    }

    private void StartCrouching()
    {
        myBody.localScale = crouchScale;
        myCameraHead.position -= new Vector3(0, 0.1615f, 0);
        myController.height /= 2;
        isCrouching = true;

        if(isRunning)
        {
            velocity = Vector3.ProjectOnPlane(myCameraHead.transform.forward, Vector3.up).normalized * slideSpeed * Time.deltaTime;
            startSliderTimer = true;
        }
    }

    private void StopCrouching()
    {
        myBody.localScale = bodyScale;
        myCameraHead.position += new Vector3(0, 0.1615f, 0);
        myController.height = initialControllerHeight;
        isCrouching = false;
        currentSlideTimer = 0f;
        velocity = new Vector3(0f, 0f, 0f);
        startSliderTimer = false;
    }

    void Jump()
    {
        readyToJump = Physics.OverlapSphere(ground.position, groundDist, groundLayer).Length > 0;

        if(Input.GetButtonDown("Jump") && readyToJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
            AudioManager.instance.PlayerSFX(2);
        }

        myController.Move(velocity);

    }

    // Contains code for player movement
    private void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        /*float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;*/

        Vector3 movement = x * transform.right + z * transform.forward;

        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            movement = movement * runSpeed * Time.deltaTime;
            isRunning = true;
        }
        else if(isCrouching)
            movement = movement * crouchSpeed * Time.deltaTime;
        else
        {
            movement = movement * speed * Time.deltaTime;
            isRunning = false;
        }

        /*cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        myCameraHead.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);*/

        //myAnimator.SetFloat("PlayerSpeed", movement.magnitude);
        movement += flyingCharacterMomentum * Time.deltaTime;

        if (movement.magnitude > 0 && movement.magnitude < sprintSpeed)
        {
            myAnimator.SetBool("Walking", true);
            myAnimator.SetBool("Sprinting", false);
        }
        else if(movement.magnitude > sprintSpeed)
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Sprinting", true);
        }
        else
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Sprinting", false);
        }

        

        myController.Move(movement);

        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;

        myController.Move(velocity);

        if (myController.isGrounded)
        {
            velocity.y = Physics.gravity.y * Time.deltaTime;
        }

        if(flyingCharacterMomentum.magnitude > 0f)
        {
            float reductionAmount = 3f;
            flyingCharacterMomentum -= flyingCharacterMomentum * reductionAmount * Time.deltaTime;

            if (flyingCharacterMomentum.magnitude < 4f)
                    flyingCharacterMomentum = Vector3.zero;
        }
        {

        }
    }

    private void CameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        myCameraHead.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }

    private void SlideCounter()
    {
        if(startSliderTimer)
        {
            currentSlideTimer += Time.deltaTime;
        }
    }

    private void HandleHookShotStart()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //sends ray out to check if hitting something
            RaycastHit hit;

            if (Physics.Raycast(myCameraHead.position, myCameraHead.forward, out hit))
            {
                //when hit something, attatch it to the variables and change to proper "state"
                hitPointTransform.position = hit.point;
                hookShotPosition = hit.point;
                state = State.HookFlyingPlayer;
            }
        }
    }

    private bool TestHookStop()
    {
        return Input.GetKey(KeyCode.Space);
    }
    
    private void HandleHookShotMovement()
    {
        //direction of movement
        Vector3 hookShotDirection = (hookShotPosition - transform.position).normalized;

        float hookShotSpeedMin = 10f, hookShotSpeedMax = 20f;

        float hookShotSpeedMod = Mathf.Clamp(Vector3.Distance(transform.position, hookShotPosition), hookShotSpeedMin, hookShotSpeedMax);

        myController.Move(hookShotDirection * hookShotSpeed * hookShotSpeedMod * Time.deltaTime);

        if (Vector3.Distance(transform.position, hookShotPosition) < 2f)
            state = State.Normal;

        if (TestHookStop())
        {
            float extraMomentum = 30f, jumpSpeedUp = 100f;
            flyingCharacterMomentum += hookShotDirection * hookShotSpeed * extraMomentum;
            flyingCharacterMomentum += Vector3.up * jumpSpeedUp;
            state = State.Normal;
        }
    }

    private void ResetGravity()
    {
        velocity.y = 0f;
    }
}
