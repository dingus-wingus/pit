using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject Cam;
    public PlayerScript PC;
    public Vector3 Offset = new Vector3(0, 0, -10);
    float hOffset = 0;
    public float LerpSpeed = 0.1f;
    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Offset = new Vector3(hOffset, 0, Offset.z);

        Cam.transform.position = PC.transform.position + Offset;

        /*if (PC.facingLeft)
        {
            hOffset = Mathf.Lerp(hOffset, -2, LerpSpeed);
        }
        else
        {
            hOffset = Mathf.Lerp(hOffset, 2, LerpSpeed);
        }*/

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, topLimit), Mathf.Clamp(transform.position.z, -10, -10));
    }
}
