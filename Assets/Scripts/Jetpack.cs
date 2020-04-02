using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    AudioSource audio;
    Rigidbody rb;
    [SerializeField] float RcsThrust = 200f; // average thrust rotation of jetpack also serialize makes it so its editable in inspector but not allowed to be edited by other scripts
    public float thrustSpeed = 1000f; // average thrust speed of jerpack

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        Thrust();
        Rotation();    
    }


    private void Thrust()
    {
        float JetSpeed = Time.deltaTime * thrustSpeed;
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * JetSpeed);
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            audio.Stop();
        }
    }
    private void Rotation()
    {
        rb.freezeRotation = true; // take manual control of rotation
        float rotationSpeed = Time.deltaTime * RcsThrust; // rotation per frame
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rb.freezeRotation = false; // resume physics control of rotation
    }

}
