using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCube : MonoBehaviour
{
    Vector3 startPos;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            StartCoroutine(ResetPos());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FallingObject")
        {
            transform.position = startPos;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void Start()
    {
        startPos = transform.position;
    }
    IEnumerator ResetPos()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position=startPos;
    }
}
