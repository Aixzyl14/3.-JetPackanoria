using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    public float JetSpeed = 20f;
    AudioSource audio;
    Rigidbody rb;
    [SerializeField] float RcsThrust = 200f; // average thrust rotation of jetpack also serialize makes it so its editable in inspector but not allowed to be edited by other scripts

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        Thrust();
        Rotation();
        if (((gameObject.transform.eulerAngles.z >= 130) && (gameObject.transform.eulerAngles.z <= 180)) || ((gameObject.transform.eulerAngles.z <= -130) && (gameObject.transform.eulerAngles.z >= -180)))
        {
            JetSpeed = -20f;
        }
        else
        {
            JetSpeed = 20f;
        }

    }


    private void Thrust()
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("friendly");
                break;
            case "Destructive":
                Destroy(gameObject);
                break;
            default:
                break;
                
        }
    }

}
