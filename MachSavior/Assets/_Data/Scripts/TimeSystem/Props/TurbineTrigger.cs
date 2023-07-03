using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbineTrigger : MonoBehaviour
{
    [SerializeField] private AnimatedTimeObject _animatedTimeObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _animatedTimeObject.IsActive())
        {
            print("Apply air force");
        }

    }
}
