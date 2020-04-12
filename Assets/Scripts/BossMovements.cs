using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovements : MonoBehaviour
{
    float JetSpeed;
    bool BossIsAlive = true;
    Rigidbody rb;
    float BossTiming = 0.5f;
    float BossRcsThrust = 200f;
    bool LeftRotationDone = false;
    bool RightRotationDone = false;
    enum BossStates { LeftRotation, RightRotation, Thrusting}
    BossStates currentState = BossStates.LeftRotation;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (BossIsAlive)
        {
            BossMovement();
        }
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
        JetSpeed = 2000f;
        rb.AddRelativeForce(Vector3.up * (JetSpeed * Time.deltaTime));
        StartCoroutine(Thrustingtime());
        if (LeftRotationDone)
        {
            currentState = BossStates.RightRotation;
            LeftRotationDone = false;
        }
        else
        {
            currentState = BossStates.LeftRotation;
            RightRotationDone = false;
        }
    }
    private void moveLeft()
    {
        if (gameObject.transform.eulerAngles.z >= 20)
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
        bool a = false;
        if (a == true)
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

    IEnumerator Thrustingtime()
    {
       yield return new WaitForSeconds(5);
        rb.freezeRotation = true;
    }
}
