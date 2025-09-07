using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerScript : MonoBehaviour
{
    Rigidbody rigidBody;
    public float speed = 10;

    public float health;
    public float maxHealth;

    public float stamina;
    public float maxStamina;

    public float grapples;
    public float maxGrapples;

    public bool facingLeft;

    //By Wade; variables for fall damage
    public LayerMask groundLayer;
    public bool wasGrounded;
    public bool wasFalling;
    public float startOfFall;
    public bool _grounded = false;
    public float minimumFall;

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
        Move();

        CheckGround();

        if (!wasFalling && isFalling) startOfFall = transform.position.y;
        if (!wasGrounded && _grounded) TakeDamage();

        wasGrounded = _grounded;
        wasFalling = isFalling;
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
            transform.rotation = Quaternion.Euler(0, 0, 0);
            facingLeft = false;
        }

        //Check if A key is held
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody>().MovePosition(transform.position + (Vector3.left * speed * Time.deltaTime));
            //transform.position += Vector3.left * speed * Time.deltaTime;
            //make facingleft false
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facingLeft = true;
        }
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
