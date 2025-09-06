using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerScript : MonoBehaviour
{
    public float speed = 10;

    public float health;
    public float maxHealth;

    public float stamina;
    public float maxStamina;

    public int grapples;
    public float maxGrapples;

    public bool facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
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
}
