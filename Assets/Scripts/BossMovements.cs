using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovements : MonoBehaviour
{
    int boost = 0;
    float JetSpeed;
    bool BossIsAlive = true;
    Rigidbody rb;
    float BossTiming = 0.5f;
    float BossRcsThrust = 200f;
    bool LeftRotationDone = false;
    bool RightRotationDone = false;
    float angle;
    [SerializeField] ParticleSystem BossDeadPart;
    enum BossStates { LeftRotation, RightRotation, Thrusting}
    BossStates currentState = BossStates.LeftRotation;
    [SerializeField] AudioClip Death;
    AudioSource audio;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        angle = gameObject.transform.eulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;
        if (BossIsAlive)
        {
            BossMovement();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!BossIsAlive) { return; }
        else
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                BossDead();
                StartCoroutine("DeathTime");
                Destroy(gameObject);
            }
        }

    }
    private void BossDead()
    {
        audio.Stop();
        audio.PlayOneShot(Death);
        BossDeadPart.Play();
    }
    private void BossMovement()
    {
        BossRotation();

    }

    private void BossRotation()
    {
        switch (currentState)
        {
            case (BossStates.LeftRotation):
                Invoke("moveLeft", BossTiming);
                break;
            case (BossStates.RightRotation):
                Invoke("moveRight", BossTiming);
                break;
            case (BossStates.Thrusting):
                Invoke("Thrusting", BossTiming);
                break;
            default:

            break;

        }
        
    }

    void Thrusting()
    {
        boost++;
        float currentboost = boost;
        JetSpeed = 2200f;
        rb.AddRelativeForce(Vector3.up * (JetSpeed * Time.deltaTime));
        if ( currentboost >= 10)
        {
            if (LeftRotationDone)
            {
                currentState = BossStates.RightRotation;
                LeftRotationDone = false;
                boost = 0;
            }
            else if (RightRotationDone)
            {
                currentState = BossStates.LeftRotation;
                RightRotationDone = false;
                boost = 0;
            }
            else { }
        }
        else { }
    }
    private void moveLeft()
    {
        if (angle >= 30)
        {
            rb.freezeRotation = true;
            currentState = BossStates.Thrusting;
            LeftRotationDone = true;
            return;
        }
        else
        {
            float rotationSpeed = Time.deltaTime * BossRcsThrust;
            transform.Rotate(Vector3.forward * rotationSpeed);
        }

    }

    void moveRight()
    {
        if (angle <= -30)
        {
            rb.freezeRotation = true;
            currentState = BossStates.Thrusting;
            RightRotationDone = true;
            return;
        }
        else
        {
            float rotationSpeed = Time.deltaTime * BossRcsThrust;
            transform.Rotate(Vector3.back * rotationSpeed);
        }
    }

    IEnumerator DeathTime()
    {
       yield return new WaitForSeconds(3);
    }
}
