using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] float period = 2f;
    [SerializeField] Vector3 MovementVector = new Vector3(10f, 10f, 10f);

    float movementFactor; // 0 for not moved, 1 for fully moved
    Vector3 startingPos; // must be stored for absolute  movement


    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        if (period <= Mathf.Epsilon) { print("Period cannot be 0"); return; }

            float cycles = Time.time / period; //grows continually from zero

            const float tau = Mathf.PI * 2; //2 pi for full cycle
            float rawSineWave = Mathf.Sin(cycles * tau);
             
            movementFactor = rawSineWave / 2f + 0.5f; //half amplitude but then need to add 0.5 to get a range of -1 to 1 treat as double
            Vector3 offsetMov = MovementVector * movementFactor;
            transform.position = startingPos + offsetMov;

    }
}
