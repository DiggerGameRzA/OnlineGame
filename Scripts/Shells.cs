using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shells : MonoBehaviour
{
    float alive_time = 10f;         //Time of alive of this shell
    float shells_force = 10f;       //Force of this shell
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        alive_time -= Time.deltaTime;   //Countdown alive time
        if (alive_time <= 0)
        {
            Destroy(this.gameObject);   //Destroy this shell when alive time is equal to 0 or less
        }
    }
    void FixedUpdate()
    {
        rb.AddForce(transform.right * shells_force);
    }
}

