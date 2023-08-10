using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //Player movement control
    playerAudio audioScript;
    public CharacterController pController;
    private Vector3 moveVec = Vector3.zero;
    private Animator artist;
    private orientationIconBehaviour mapCursor;


    public float speed = 5f;
    public float jumpHeight = 2.0f;
    private float gravity = -9.18f;
    private float rSpeed = 2f;
    private bool canDefend = false;
    public bool canMove = true;
    private bool groundedPlayer = true;
    

    private void Start()
    {
        audioScript = GetComponent<playerAudio>();
        artist = GetComponent<Animator>();
    }


    void Update()
    {

        //Rotation Input
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up, mouseInput.x * rSpeed);
      
        if (groundedPlayer)
        {
            //Reset and animation
            moveVec.y = 0f;
            artist.SetBool("isJumping", false);
            artist.SetBool("grounded", true);

            if (canMove)
            { 
                //x,z movement
                moveVec = CalcMove();
                
                //jumping
                if (Input.GetButtonDown("Jump"))
                {
                    artist.SetBool("isJumping", true);
                    artist.SetBool("grounded", false);

                    moveVec.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                }
            }
           
        }

       

        //apply gravity and move
        moveVec.y += gravity * Time.deltaTime;
        pController.Move(moveVec * speed * Time.deltaTime);
        groundedPlayer = pController.isGrounded;
        
    }

    //Calculating x,z movement
    Vector3 CalcMove()
    {
        
        float rawH = Input.GetAxisRaw("Horizontal");
        float rawV = Input.GetAxisRaw("Vertical");

        //animation
        artist.SetFloat("X", rawH);
        artist.SetFloat("Z", rawV);
        if ((rawH != 0) || (rawV != 0))
        {
            audioScript.startFootsteps();
            artist.SetBool("isMoving", true);
            canDefend = false;
        }
        else
        {
            audioScript.endFootsteps();
            artist.SetBool("isMoving", false);
            canDefend = true;

        }
        if (Input.GetButton("Sprint") && (rawH == 0) && (rawV != -1))
        {
            speed = 10f;
        }
        else
        {
            speed = 5f;
        }

        //Compose vector for x,z
        Vector3 v = new Vector3(rawH, 0f, rawV).normalized;
        Vector3 mV = transform.TransformDirection(v);

        return mV;
    }

    //Gets, Sets 

    public void setCanMove(bool enabled)
    {
        if(enabled)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }
     
    public bool getIfCanDefend()
    {
        return canDefend;

    }

    public bool getIfGroundedPlayer()
    {
        return groundedPlayer;
    }


}



