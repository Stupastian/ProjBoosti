using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip finishJingle;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem explosionParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    //Since there are only 2 states, better solution would be simple boolean variable isTransitioning
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    bool collisionDisabled = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if(Debug.isDebugBuild){
            RespondToDebugKey();
        }
    }

    private void RespondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive || !collisionDisabled) { return; }

        switch (collision.gameObject.tag)
        {
        case "Friendly":
            //do nothing
            break;
        case "Finish":
            StartLandingSequence();
            break;
        default:
            StartDeathSequence();
            break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        explosionParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartLandingSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(finishJingle);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneindex + 1 >= SceneManager.sceneCountInBuildSettings){
            LoadFirstLevel();
        } else
        {
            SceneManager.LoadScene(currentSceneindex + 1);
        }
    }


    private void RespondToRotateInput()
    {

        


        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

            RotateManually(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RotateManually(-rotationThisFrame);
        }

    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}
