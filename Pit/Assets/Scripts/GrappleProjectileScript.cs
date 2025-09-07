using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectileScript : MonoBehaviour
{
    public float projectileSpeed = 25;
    public Vector3 directionVector;
    public PlayerScript player;
    public float lifetime = 1f;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        directionVector = directionVector.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(this.gameObject);
            player.StartGrappling();
        }

        Ray ray = new Ray(transform.position, directionVector);

        if (Physics.Raycast(ray, projectileSpeed * Time.deltaTime))
        {
            transform.position = (transform.position + (directionVector * projectileSpeed * Time.deltaTime));
            projectileSpeed = 0;

            Destroy(this.gameObject);
            player.StartGrappling();
        }

        transform.position = (transform.position + (directionVector * projectileSpeed * Time.deltaTime));

        player.grappleEndPos = transform.position;
        player.grappleStartPos = player.transform.position;
        player.SetGrapple();

        
    }
}
