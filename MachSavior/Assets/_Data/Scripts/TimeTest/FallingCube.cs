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
