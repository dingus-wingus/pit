using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerScript : MonoBehaviour
{
    Rigidbody rigidBody;
    public float speed = 10;
    public float climbSpeed = 3;

    public float health;
    public float maxHealth;

    public float stamina;
    public float maxStamina;
    public float staminaDrain; //the rate at which stamina drains in stamina per second
    public float staminaRecover; //the rate at which stamina is recovered in stamina per second

    public float grapples;
    public float maxGrapples;

    public bool climbing;

    public bool facingLeft;

    //By Wade; variables for fall damage
    public LayerMask groundLayer;
    public bool wasGrounded;
    public bool wasFalling;
    public float startOfFall;
    public bool _grounded = false;
    public float minimumFall;

    //Player State Machine
    private enum playerState { //this is an enumerator, a type of variable that can be any value from a defined set. All of our different states are defined here
        normal, //moving, falling, etc
        climbing, //climbing
    }
    private playerState currentState = playerState.normal;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        switch (currentState) //this is a switch statement, we can define cases for it to check that if true, will execute all code beneath it to the END of the entire switch state
        {
            case playerState.normal:
                Move();

                CheckGround();

                if (!wasFalling && isFalling) startOfFall = transform.position.y;
                if (!wasGrounded && _grounded) TakeDamage();

                wasGrounded = _grounded;
                wasFalling = isFalling;

                //Recover stamina
                if (stamina < maxStamina && _grounded)
                {
                    stamina += staminaRecover * Time.deltaTime;
                    if (stamina > maxStamina) stamina = maxStamina;
                }

                break; //by using the break keyword we can tell the switch state when to stop executing code
                       //without this if playerstate == playerstate.normal, the code for case playerstate.climbing would also be executed
            case playerState.climbing:
                Climb();

                stamina -= staminaDrain * Time.deltaTime;
                if (stamina < 0) stamina = 0;

                break;
        }
        
    }

    private void Climb()
    {
        Vector3 climbVector = Vector3.zero;
        //Check if D key is held
        if (Input.GetKey(KeyCode.D))
        {
            climbVector += Vector3.right;
            //transform.position += Vector3.right * speed * Time.deltaTime;
            //make facingLeft true
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            facingLeft = false;
        }

        //Check if A key is held
        if (Input.GetKey(KeyCode.A))
        {
            climbVector += Vector3.left;
            //transform.position += Vector3.left * speed * Time.deltaTime;
            //make facingleft false
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            facingLeft = true;
        }

        //Check if W key is held
        if (Input.GetKey(KeyCode.W))
        {
            climbVector += Vector3.up;
            //transform.position += Vector3.right * speed * Time.deltaTime;
            //make facingLeft true
            //transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //Check if S key is held
        if (Input.GetKey(KeyCode.S))
        {
            climbVector += Vector3.down;
            //transform.position += Vector3.left * speed * Time.deltaTime;
            //make facingleft false
            //transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        climbVector = climbVector.normalized;
        climbVector *= climbSpeed;

        GetComponent<Rigidbody>().MovePosition(transform.position + (climbVector * Time.deltaTime));


        if (!Input.GetKey(KeyCode.Space) || stamina <= 0) //if the player lets go of space
        {
            GetComponent<Rigidbody>().useGravity = true;
            currentState = playerState.normal; //return to normal state
        }
    }

    /// <summary>
    /// Check for input to move the player left or right
    /// </summary>
    private void Move()
    {
        //Check if D key is held
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody>().MovePosition(transform.position + (Vector3.right * speed * Time.deltaTime));
            //transform.position += Vector3.right * speed * Time.deltaTime;
            //make facingLeft true
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            facingLeft = false;
        }

        //Check if A key is held
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody>().MovePosition(transform.position + (Vector3.left * speed * Time.deltaTime));
            //transform.position += Vector3.left * speed * Time.deltaTime;
            //make facingleft false
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            facingLeft = true;
        }

        if(Input.GetKey(KeyCode.Space) && _grounded)
        {
            GetComponent<Rigidbody>().useGravity = false;
            currentState = playerState.climbing;
        }

        /* Sean commented this out because the code for climbing is now in the Climb() Function
        if(climbing == true)
        {

            if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Rigidbody>().MovePosition(transform.position + (Vector3.right * climbSpeed * Time.deltaTime));
                //transform.position += Vector3.right * speed * Time.deltaTime;
                //make facingLeft true
                transform.rotation = Quaternion.Euler(0, 0, 0);
                facingLeft = false;
            }

            //Check if A key is held
            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody>().MovePosition(transform.position + (Vector3.left * climbSpeed * Time.deltaTime));
                //transform.position += Vector3.left * speed * Time.deltaTime;
                //make facingleft false
                transform.rotation = Quaternion.Euler(0, 180, 0);
                facingLeft = true;
            }

            if (Input.GetKey(KeyCode.W))
            {
                GetComponent<Rigidbody>().MovePosition(transform.position + (Vector3.up * climbSpeed * Time.deltaTime));
                //transform.position += Vector3.right * speed * Time.deltaTime;
            }

            //Check if A key is held
            if (Input.GetKey(KeyCode.S))
            {
                GetComponent<Rigidbody>().MovePosition(transform.position + (Vector3.down * climbSpeed * Time.deltaTime));
                //transform.position += Vector3.left * speed * Time.deltaTime;
            }
            
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrapplePickupScript>())
        {
            grapples = Mathf.Clamp(grapples + other.GetComponent<GrapplePickupScript>().grappleGiven, 0, maxGrapples);
            Destroy(other.gameObject);
        }

        if (other.GetComponent<HealthPickupScript>())
        {
            health = Mathf.Clamp(health + other.GetComponent<HealthPickupScript>().healthGiven, 0, maxHealth);
            Destroy(other.gameObject);
        }

        if (other.GetComponent<VineScript>())
        {
            speed = 2;
            GetComponent<Rigidbody>().drag = 12;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<VineScript>())
        {
            speed = 10;
            GetComponent<Rigidbody>().drag = 0;
        }
    }
    /// <summary>
    /// Author: Wade
    /// Desc: Manages player fall time and corresponding fall damage, if any.
    /// </summary>
    public void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        _grounded = Physics.Raycast(ray, 1.1f, groundLayer);
    }

    bool isFalling { get { return (!_grounded && rigidBody.velocity.y < 0); } }

    /// <summary>
    /// Author: Wade
    /// Desc: Manages when player takes damage
    /// </summary>
    /// 
    private void TakeDamage()
    {
        float fallDistance = startOfFall - transform.position.y;

        if (fallDistance > minimumFall)
        {
            health = ((health) - (fallDistance * 3));
            Debug.Log("Player fell " + fallDistance + " units ");
        }


    }
}
