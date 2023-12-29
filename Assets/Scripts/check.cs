using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check : MonoBehaviour
{
    private Vector3 startPos;
    public float maxSpeed;
    private Rigidbody rb;
    public LayerMask whatIsRoad;
    bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics.Raycast(rb.position, -Vector3.up, 5f);
        if (!grounded){
            transform.position = startPos;
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }
}
