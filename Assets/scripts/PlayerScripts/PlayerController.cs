using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("External Variables")]
    public RoadManager roadManager;
    public cameraShake camShake;
    public Timer timer;
    public float speedUpCDTimerMax;
    public float speedUpTimerMax; //option for alternate timer so speed up and speed up cooldown last for differing times
    public float speedUpTimer;
    public float speedUpMultiplier;
    public bool canSpeed;
    public float centrifugalForceMultiplier = 0.3f;


    [Header("Player Road Move Variables")]
    [SerializeField] float hitSpeed;
    [SerializeField] float hitSpeedTimerMax;
    [SerializeField] float gradualSpeedMultiplier;
    [SerializeField] float timerGradSpeedMax;
    [SerializeField] float speedUpTimerMax;
    [SerializeField] float speedUpTimer;
    [SerializeField] AudioSource hitNoise;
    public bool hitObstacle;

    [Header("Player Horizontal Move Variables")]
    public Rigidbody2D myBody;
    public Transform leftScreen;
    public Transform rightScreen;
    public int HorzMovement;
    public float horzSpeed = 5f;
    public float horzIncriment = .75f;

    [Header("Input Buttons and Keys")]
    public KeyCode KeyRight;
    public KeyCode KeyLeft;
    public KeyCode KeyUp;

    [Header("Internal Player Variables")]
    public float healthMax;
    public float health;
    public bool speedUp;

<<<<<<< HEAD:Assets/scripts/PlayerScripts/PlayerController.cs
    [Header ("Private Variables")]
=======
    private Rigidbody2D myBody;
    private PlayerInput playerInput; 
  

    float moveHorizontal = 0f;
>>>>>>> main:Assets/scripts/PlayerController.cs
    float hitSpeedTimer;
    float timerGradSpeed;
    float moveSpeed;
    //Player anim parameters
    bool leftPressed = false;
    bool rightPressed = false;
<<<<<<< HEAD:Assets/scripts/PlayerScripts/PlayerController.cs
=======
    bool canSpeedUp; 
   
>>>>>>> main:Assets/scripts/PlayerController.cs

    Animator myAnim;

    SpriteRenderer myRend;

<<<<<<< HEAD:Assets/scripts/PlayerScripts/PlayerController.cs

  
=======
>>>>>>> main:Assets/scripts/PlayerController.cs
    // Start is called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();

        myAnim = gameObject.GetComponent<Animator>();
        myRend = gameObject.GetComponent<SpriteRenderer>();


        health = healthMax;
        timerGradSpeed = timerGradSpeedMax;
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD:Assets/scripts/PlayerScripts/PlayerController.cs
        MoveAnim(); 
        MoveInput();
=======

        if (moveHorizontal < 0) // turning left
        {
            myAnim.SetBool("leftHold", true); // continues to hold button down, transitions to holding anim 
            if (leftPressed == false) // first frame
            {
                leftPressed = true;
                myAnim.SetTrigger("leftPressed"); // anim activated one time 
            }
            rightPressed = false;
            myAnim.SetBool("rightHold", false);
            myAnim.ResetTrigger("rightPressed");
            //myAnim.ResetTrigger("leftPressed");
        }
        else if (moveHorizontal > 0) // turning right
        {
            myAnim.SetBool("rightHold", true); // continues to hold button down, transitions to holding anim 
            if (rightPressed == false) // first frame
            {
                rightPressed = true;
                myAnim.SetTrigger("rightPressed"); // anim activated one time 
            }
            leftPressed = false;
            myAnim.SetBool("leftHold", false);
            //myAnim.ResetTrigger("rightPressed");
            myAnim.ResetTrigger("leftPressed");

        }
        else if (moveHorizontal == 0) //reset all values/directions 
        {
            leftPressed = false;
            rightPressed = false;
            myAnim.ResetTrigger("rightPressed");
            myAnim.ResetTrigger("leftPressed");
            myAnim.SetBool("leftHold", false);
            myAnim.SetBool("rightHold", false);
        }
        
        //implement speed up 
        if (!canSpeedUp && speedUpTimer >= 0)
        {
            speedUpTimer--;
        }
        else if (speedUpTimer <= speedUpTimerMax)
        {
            canSpeedUp = true;
        }
        if (speedUp)
        {
            speedUpTimer++;
            if (speedUpTimer >= speedUpTimerMax)
            {
            
                speedUp = false;
                canSpeedUp = false;
                speedUpTimer = speedUpTimerMax;
            
            }
        }
      
        

        /*

        if (Input.GetKeyDown(KeyCode.W))
        { 
            speedUp = true;
        }
        else if (Input.GetKeyUp(KeyCode.W)) 
        {
            speedUp = false; 
        }
        //if (hitObstacle == true) { myAnim.SetBool("hitobstacle", false); }
        else { myAnim.SetBool("hitObstacle", false); }
        */ 

       //NEW INPUT SETTINGS 
       myBody.velocity = new Vector2(moveHorizontal * moveSpeed, myBody.velocity.y);
>>>>>>> main:Assets/scripts/PlayerController.cs
    }
    void FixedUpdate()
    {
        playerMove();

        #region gradual speed up
        if (timerGradSpeed <= 0)
        {
            roadManager.normSpeed += gradualSpeedMultiplier;
            timer.maxSpeed += gradualSpeedMultiplier;
            timer.minSpeed += gradualSpeedMultiplier;
            timerGradSpeed = timerGradSpeedMax;
            //Debug.Log(timer.normSpeed);
        }
        else { timerGradSpeed--; }
        #endregion

<<<<<<< HEAD:Assets/scripts/PlayerScripts/PlayerController.cs
=======
        
        #region implement movement
        Vector2 netHorizontalForce;
        netHorizontalForce = Vector2.zero;



        if (moveHorizontal != 0)
        {
            //myBody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse); 
            // not using Time.Delta time beacause AddForce has it applied by default. 
            netHorizontalForce += new Vector2(moveHorizontal * moveSpeed, 0f);
        }
        #endregion
        

>>>>>>> main:Assets/scripts/PlayerController.cs
        #region norm speed
        if (!hitObstacle)
        {
            if (roadManager.speed <= roadManager.normSpeed)
            {
                roadManager.speed++;
            }
            if (speedUp && roadManager.speed <= roadManager.maxSpeed)
            {
                //Debug.Log(speedUp);
                roadManager.speed += speedUpMultiplier;
            }
            if (!speedUp)
            {
                if (roadManager.speed >= roadManager.normSpeed)
                {
                    roadManager.speed -= speedUpMultiplier * 0.5f;
                }
            }
        }
        #endregion

        #region collide speed
        if (hitObstacle)
        {
            if (hitSpeedTimer > 0)
            {
                hitNoise.Play();
                roadManager.speed = hitSpeed;
                camShake.CameraShake();
                myAnim.SetBool("hitAnim", true);
                speedUpTimer = speedUpTimerMax;
                hitSpeedTimer--;
                
            }
            if (hitSpeedTimer <= 0)
            {
                health--;
                camShake.StopShake();
                myAnim.SetBool("hitAnim", false);
                hitObstacle = false;
                
            }
        }
        #endregion

<<<<<<< HEAD:Assets/scripts/PlayerScripts/PlayerController.cs
        /* Centrifugal Force
        #region apply centrifugal force
=======
        /* OLD INPUT SETTINGS 
        #region apply speed
>>>>>>> main:Assets/scripts/PlayerController.cs

       
        float ZPos = roadManager.ZPos;
        
        if (roadManager.FindSegment(ZPos).curviness != 0)
        {
            //myBody.AddForce(new Vector2(-roadManager.FindSegment(ZPos).curviness * centrifugalForceMultiplier, 0f), ForceMode2D.Impulse);

            
            //This little bit is what we added!

            
            if (roadManager.FindSegment(ZPos).index > roadManager.segmentToCalculateLoopAt)
            {
                netHorizontalForce += new Vector2(-roadManager.endSegments[roadManager.FindSegment(ZPos).index - roadManager.segmentToCalculateLoopAt].curviness * Mathf.Pow(centrifugalForceMultiplier, 2) * roadManager.speed, 0f);
            }
            else
            {
                netHorizontalForce += new Vector2(-roadManager.FindSegment(ZPos).curviness * Mathf.Pow(centrifugalForceMultiplier, 2) * roadManager.speed, 0f);
            }


            Debug.Log(-roadManager.FindSegment(ZPos).curviness);
        }

        myBody.AddForce(netHorizontalForce, ForceMode2D.Impulse);
<<<<<<< HEAD:Assets/scripts/PlayerScripts/PlayerController.cs
            
        #endregion
        */
=======
        
        #endregion
        */

>>>>>>> main:Assets/scripts/PlayerController.cs
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "staticObstacle")
        {
            hitObstacle = true;
            hitSpeedTimer = hitSpeedTimerMax;
        }
    }

<<<<<<< HEAD:Assets/scripts/PlayerScripts/PlayerController.cs
    public void playerMove()
    {
        //Slidey movement
        if (moveSpeed < horzSpeed * HorzMovement) { moveSpeed += horzIncriment; }
        else if (moveSpeed > horzSpeed * HorzMovement) { moveSpeed -= horzIncriment; }
        

        //apply movement to rigid body
        myBody.velocity = new Vector3(moveSpeed, 0f, 0f);
    }
    public void MoveInput()
    {
        //HORIZONTAL
        //Get Input && Define HorzMovement
        HorzMovement = 0; //defaults to zero
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //convert mouse position from screed cord. to word cord. 
        if (Input.GetKey(KeyRight) || /*TouchInput*/ (Input.GetMouseButton(0) && (mousePos.x > rightScreen.position.x))) //right input
        {
            HorzMovement = 1;
        }

        if (Input.GetKey(KeyLeft) || /*TouchInput*/ (Input.GetMouseButton(0) && (mousePos.x < leftScreen.position.x))) //left input
        {
            HorzMovement = -1;
        }

        //SPEEDUP
        if (speedUpTimer <= 0)
        {
            canSpeed = true;
        }

        if (canSpeed && (Input.GetKeyDown(KeyCode.W) || (Input.GetMouseButton(0) && ((mousePos.x < rightScreen.position.x) && (mousePos.x > leftScreen.position.x)))))
        {
            speedUp = true;
            canSpeed = false;
        } 

        if (speedUp && speedUpTimer <= speedUpCDTimerMax)
        {
            speedUpTimer++;
        }

        if (speedUpTimer >= speedUpCDTimerMax)
        {
            speedUp = false;
        } if (speedUpTimer >= 0 && !speedUp)
        {
            speedUpTimer--;
        }

    }
    public void MoveAnim()
    {
        if (HorzMovement < 0) // turning left
        {
            myAnim.SetBool("leftHold", true); // continues to hold button down, transitions to holding anim 
            if (leftPressed == false) // first frame
            {
                leftPressed = true;
                myAnim.SetTrigger("leftPressed"); // anim activated one time 
            }
            rightPressed = false;
            myAnim.SetBool("rightHold", false);
            myAnim.ResetTrigger("rightPressed");
            //myAnim.ResetTrigger("leftPressed");
        }
        else if (HorzMovement > 0) // turning right
        {
            myAnim.SetBool("rightHold", true); // continues to hold button down, transitions to holding anim 
            if (rightPressed == false) // first frame
            {
                rightPressed = true;
                myAnim.SetTrigger("rightPressed"); // anim activated one time 
            }
            leftPressed = false;
            myAnim.SetBool("leftHold", false);
            //myAnim.ResetTrigger("rightPressed");
            myAnim.ResetTrigger("leftPressed");

        }
        else if (HorzMovement == 0) //reset all values/directions 
        {
            leftPressed = false;
            rightPressed = false;
            myAnim.ResetTrigger("rightPressed");
            myAnim.ResetTrigger("leftPressed");
            myAnim.SetBool("leftHold", false);
            myAnim.SetBool("rightHold", false);
        }
        //if (hitObstacle == true) { myAnim.SetBool("hitobstacle", false); }
        else { myAnim.SetBool("hitObstacle", false); }
=======
    public void Move (InputAction.CallbackContext context)
    {
        moveHorizontal = context.ReadValue<Vector2>().x; //NEW INPUT SETTINGS 
    }
    public void Speedup (InputAction.CallbackContext context)
    {
        if (context.performed && canSpeedUp)
        {
            speedUp = true;
        }
>>>>>>> main:Assets/scripts/PlayerController.cs
    }

}
