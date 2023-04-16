using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsColliderTest : MonoBehaviour
{
    public Transform point;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "FallingObject")
        {
            other.transform.position = new Vector3(other.transform.position.x, point.transform.position.y, point.transform.position.z);
            other.rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }
}
