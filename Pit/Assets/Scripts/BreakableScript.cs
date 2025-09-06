using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerScript>())
        {
            StartCoroutine(BreakUp());
        }
    }

    IEnumerator BreakUp()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
