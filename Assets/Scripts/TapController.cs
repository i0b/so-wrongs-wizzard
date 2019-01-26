using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    public float tapForce = 300;
    public float tiltSmooth = 5;
    //public Vector3 startPosition;

    Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -50);
        forwardRotation = Quaternion.Euler(0, 0, 20);
        // rigidbody.simulated = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector3.up * tapForce, ForceMode2D.Force);
        }
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ScoreZone") { 
            // score update
        }
        if(collision.gameObject.tag == "DeadZone") {
            rigidbody.simulated = false;
        }
    }
}
