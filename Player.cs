using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Variables
    CharacterController controller;

    //#####For movement#####
    //Speed control
    public float movement;
    public float speed = 6.0F;
    public float speed_rotation = 4.0F;
    public float jumpSpeed = 100.0F;
    public float gravity = 100.0F;
    private Vector3 moveDirection = Vector3.zero;


    //#####For animating

    //Animator
    public Animator play;
    //states
    public bool forward = false;
    public bool backwards = false;
    public bool sidewards = false;
    public bool standing = false;

    private bool pressed_c_button = false;//Überprüft ob c schon gedrückt worden ist
    private bool pressed_shift_button = false;//Überprüft ob shift gedrückt worden ist


    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        play = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        Cursor.visible = false;//Cursor verstecken
        keyboardcontrol();
        animating();
    }


    void keyboardcontrol()
    {
        
        //Beenden
        if (Input.GetKeyDown(KeyCode.Escape))//Beenden des Spiels (funktioniert nur im Build)
        {
            Application.Quit();
        }


        // ##### WASD-control ######

        if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D)))
        {

            if ((Input.GetKeyDown(KeyCode.W)))
            {
                forward = true;
                
            }
            else if ((Input.GetKey(KeyCode.S)))
            {
                backwards = true;
            }
            else if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
            {
                sidewards = true;
            }
            standing = false;
        }


        if (Input.GetKeyUp(KeyCode.W))
        {
            forward = false;
            
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            backwards = false;
        }

        if (Input.GetKeyUp(KeyCode.A)&&!Input.GetKey(KeyCode.D))
        {
            sidewards = false;
        }

        if (Input.GetKeyUp(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            sidewards = false;
        }


        if(!Input.GetKey(KeyCode.W)&& !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            standing = true;
        }


        if (forward == true||backwards==true||sidewards==true)
        {
            moving();
        }

        //Mause-Drehung
        var x_position = Input.GetAxis("Mouse X") * Time.deltaTime * 150.0f;
        transform.Rotate(0, x_position, 0);
    }



    void animating()
    {
        // ##### Animation ######

        if (standing == true)//Beim Loslassen von W,A,D
        {
            play.Play("Idle");
        }

        //Walking
        if (forward==true&&standing==false)//Beim Drücken von W,A,D
        {
            play.Play("Walking");
        }

        if(sidewards==true&& standing == false)
        {
            play.Play("Sidewards");
        }


       

        if (backwards==true&&standing==false)//Beim Drücken von S
        {
            play.Play("Backwards");
        }



        //Sprinten
        if (((Input.GetKey(KeyCode.LeftShift)) && pressed_shift_button == false) && (Input.GetKey(KeyCode.W)))
        {
            play.PlayInFixedTime("Sprinting");
            pressed_shift_button = true;
            speed = 2 * speed; //Verdopplund der Geschwindigkeit
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && pressed_shift_button == true)
        {
            play.Play("Walking");
            pressed_shift_button = false;
            speed = speed / 2;
        }



        //Crouching
        if ((Input.GetKeyDown(KeyCode.C) && pressed_c_button == false))
        {
            play.Play("Crouch");//in die Crouch Position
            pressed_c_button = true;
        }
        else if ((Input.GetKeyDown(KeyCode.C) && pressed_c_button == true))
        {
            play.Play("idle");//wieder in normale Position zurück
            pressed_c_button = false;
        }
    }



    public void moving()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));//Feed moveDirection with input.
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;//Multiply it by speed.
            if (Input.GetKeyDown(KeyCode.Space)) //Jumping
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;//Applying gravity to the controller
        controller.Move(moveDirection * Time.deltaTime);//Making the character move
    }




}
