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
    [SerializeField] ParticleSystem CheatPart;
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    bool CollisionDisabled = false;
    bool Cheat = false;
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
        if (Debug.isDebugBuild) // only works for development build and untick it when giving to player after
        {
            RespondToDebugInputs(); 
        }
    }

    void RespondToDebugInputs()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            CollisionDisabled = !CollisionDisabled;
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
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        rb.freezeRotation = true; // take manual control of rotation
        float rotationSpeed = Time.deltaTime * RcsThrust; // rotation per frame
        if (currentScene == 7)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Rotate(Vector3.right * rotationSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Rotate(Vector3.left * rotationSpeed);
            }
        }
        else { }
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
        if ((state != State.Alive) || (CollisionDisabled)) { return; } // ignore collision if players dead


        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                LevelComplete();
                break;
            case "Safe":

                break;
            case "CheatPoint":
                CheatPointSeq();
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

    void CheatPointSeq()
    {
        Cheat = true;
        audio.Stop();
        state = State.Transcending;
        audio.PlayOneShot(NewLevel);
        CheatPart.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void RestartFirstLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = 0;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (Cheat)
        {
                nextSceneIndex = currentScene + 2;
                LoadCheatLevel(nextSceneIndex);
                return;
        }
        else
        {
            nextSceneIndex = currentScene + 1;
        }
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        } 
            SceneManager.LoadScene(nextSceneIndex);
    }

    void LoadCheatLevel(int nextSceneIndex)
    {
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

        }
        else { }
        SceneManager.LoadScene(nextSceneIndex);
        Cheat = false;
    }
}
