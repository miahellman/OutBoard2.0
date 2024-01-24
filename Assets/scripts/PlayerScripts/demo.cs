using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class demo : MonoBehaviour
{
    [Header("Input Buttons and Keys")]
    public KeyCode KeyRight;
    public Button ButtRight;
    public KeyCode KeyLeft;
    public Button ButtLeft;
    public KeyCode KeyUp;
    public Button ButtUp;

    public int HorzMovement;
    public float moveSpeed = 5; 

    // Start is called before the first frame update
    void Start()
    {
        //Get Input && Define HorzMovement
        if ((Input.GetKey(KeyRight)) || (Input.GetButton("ButtRight")) /* || touch controls */) //right input
        {
            HorzMovement = 1;
        }

        else if ((Input.GetKey(KeyLeft)) || (Input.GetButton("ButtLeft")) /* || touch controls */) //left input
        {
            HorzMovement = -1;
        }
        else
        {
            HorzMovement = 0;
        }
        /*speedup input here */

        

    }

    // Update is called once per frame
    void Update()
    {
        playerMove(); 
    }

    void playerMove () 
    {
        //Apply Movement Speed
        Vector2 netHorizontalForce;
        netHorizontalForce = Vector2.zero;


        if (HorzMovement > 0f || HorzMovement < -0f)
        {
            //myBody.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse); 
            // not using Time.Delta time beacause AddForce has it applied by default. 
            netHorizontalForce += new Vector2(HorzMovement * moveSpeed, 0f);
        }
    }
}
