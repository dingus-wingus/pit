using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject Cam;
    public PlayerScript PC;
    public Vector3 Offset = new Vector3(0, 0, 10);
    float hOffset = 0;
    public float LerpSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Offset = new Vector3(hOffset, 0, Offset.z);

        Cam.transform.position = PC.transform.position + Offset;

        if (PC.facingLeft)
        {
            hOffset = Mathf.Lerp(hOffset, -2, LerpSpeed);
        }
        else
        {
            hOffset = Mathf.Lerp(hOffset, 2, LerpSpeed);
        }
    }
}
