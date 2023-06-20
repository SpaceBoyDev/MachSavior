using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BulletLogic>())
        {
            // TO DO break the object here
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
