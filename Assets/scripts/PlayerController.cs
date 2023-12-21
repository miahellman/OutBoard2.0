using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("External Variables")]
    public RoadManager roadManager;
    public cameraShake camShake;
    public float speedUpMultiplier;
    public Timer timer;
    public float centrifugalForceMultiplier = 0.3f;


    [Header("Internal Move Variables")]
    [SerializeField] float moveSpeed;
    [SerializeField] float hitSpeed;
    [SerializeField] float hitSpeedTimerMax;
    [SerializeField] float gradualSpeedMultiplier;
    [SerializeField] float timerGradSpeedMax;
    [SerializeField] AudioSource hitNoise;
    public bool hitObstacle;


    [Header("Player Vars")]
    public float healthMax;
    public float health;
    public bool speedUp;
    // cleanup player INPUT later

    private Rigidbody2D myBody;

    float moveHorizontal;
    float hitSpeedTimer;
    float timerGradSpeed;

    //Player anim parameters
    bool leftPressed = false;
    bool rightPressed = false;
    /*bool leftHold; 
    bool rightHold;*/

    Animator myAnim;
    SpriteRenderer myRend;



    // Start is called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();

        moveSpeed = 5f;

        myAnim = gameObject.GetComponent<Animator>();
        myRend = gameObject.GetComponent<SpriteRenderer>();


        health = healthMax;
        timerGradSpeed = timerGradSpeedMax;
    }

    // Update is called once per frame
    void Update()
    {
        #region move input
        moveHorizontal = Input.GetAxisRaw("Horizontal"); //we can change this in the Unity settings to controller in the future. 

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


        if (Input.GetKeyDown(KeyCode.W)) { speedUp = true; }
        else if (Input.GetKeyUp(KeyCode.W)) { speedUp = false; }
        //if (hitObstacle == true) { myAnim.SetBool("hitobstacle", false); }
        else { myAnim.SetBool("hitObstacle", false); }
        #endregion

        //---movement 
    
    }
    void FixedUpdate()
    {
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

        #region setSpeed
        Vector2 netHorizontalForce;
        netHorizontalForce = Vector2.zero;



        if (moveHorizontal > 0f || moveHorizontal < -0f)
        {
            //myBody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse); 
            // not using Time.Delta time beacause AddForce has it applied by default. 
            netHorizontalForce += new Vector2(moveHorizontal * moveSpeed, 0f);
        }
        #endregion

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
                    roadManager.speed -= speedUpMultiplier;
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

        #region apply speed

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
        #endregion

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "staticObstacle")
        {
            hitObstacle = true;
            hitSpeedTimer = hitSpeedTimerMax;
        }
    }
}
