using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sallanma : MonoBehaviour
{
    public float swingSpeed = 1f;     
    public float swingAngle = 30f;    

    private float initialRotationZ;
    void Start()
    {
        
        initialRotationZ = transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

        
        transform.localRotation = Quaternion.Euler(0, 0, initialRotationZ + angle);
    }
}
