using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 lastPosition;
    private float speed;
    public Vector3 speedVector;

    public float GetSpeed() { return speed; }
    public Vector3 GetSpeedVector() { return speedVector; }
    
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        speedVector = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }
}
