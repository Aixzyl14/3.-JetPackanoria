using UnityEngine;
using UnityEngine.SceneManagement;

public class Jetpack : MonoBehaviour
{
    public float JetSpeed;
    AudioSource audio;
    Rigidbody rb;
    [SerializeField] float RcsThrust = 200f; // average thrust rotation of jetpack also serialize makes it so its editable in inspector but not allowed to be edited by other scripts
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip Death;
    [SerializeField] AudioClip NewLevel;

    [SerializeField] ParticleSystem mainEnginePart;
    [SerializeField] ParticleSystem successPart;
    [SerializeField] ParticleSystem explosionPart;
    enum State { Alive, Dying, Transcending}
    State state = State.Alive;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotationInpout();
         
        }
    }


    private void RespondToThrustInput()
    {
            JetSpeed = 1200f;
           BackWardsThrust();
        if (Input.GetKey(KeyCode.Space))
        {
            Thrusting();
        }
        else
        {
            audio.Stop();
            mainEnginePart.Stop();
        }
    }

    private void Thrusting()
    {
        rb.AddRelativeForce(Vector3.up * (JetSpeed * Time.deltaTime));
        if (!audio.isPlaying)
        {
            audio.PlayOneShot(MainEngine);
            mainEnginePart.Play();
        }
    }

    private void BackWardsThrust()
    {
        if (((gameObject.transform.eulerAngles.z >= 130) && (gameObject.transform.eulerAngles.z <= 180)) || ((gameObject.transform.eulerAngles.z <= -130) && (gameObject.transform.eulerAngles.z >= -180)))
        {
            JetSpeed = -1200f;
        }

    }

    private void RespondToRotationInpout()
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
        if(state != State.Alive)  {return;} // ignore collision if players dead

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                LevelComplete();
                break;
            default:
                DeathSequence();
                break;
                
        }
    }

    private void DeathSequence()
    {
        audio.Stop();
        state = State.Dying;
        audio.PlayOneShot(Death);
        explosionPart.Play();
        Invoke("RestartFirstLevel", levelLoadDelay);
    }

    private void LevelComplete()
    {
        audio.Stop();
        state = State.Transcending;
        audio.PlayOneShot(NewLevel);
        successPart.Play();
        Invoke("LoadNextLevel", levelLoadDelay); // invoke the subroutine after 1f = 1second wait
    }

    void RestartFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
