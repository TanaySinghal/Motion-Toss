using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    [SerializeField]
    private Target target;

    [SerializeField]
    private AudioClip onGrabSound;
    [SerializeField]
    private AudioClip onThrowSound;
    [SerializeField]
    private AudioClip onAppearSound;

    AudioSource audioSource;

    private Vector3 startPosition;
    private Rigidbody myRigidbody;

    public delegate void Action();
    public event Action OnBallDied;

    private void Start()
    {
        startPosition = transform.position;
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ResetBall()
    {
        audioSource.clip = onAppearSound;
        audioSource.Play();

        transform.position = startPosition;
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
        CancelInvoke();
    }

    private void MissedBall()
    {
        Debug.Log("Missed ball");
        ResetBall();
        OnBallDied(); // Tell everyone ball died
    }

    Vector3 startThrowPosition;
    float startThrowTime;
    public void GraspBegin()
    {
        Debug.Log("Begun grab");

        audioSource.clip = onGrabSound;
        audioSource.Play();

        startThrowPosition = transform.position;
        CancelInvoke();
    }

    Queue<Vector3> velocityHistory = new Queue<Vector3>();
    public void GraspStay()
    {
        if (myRigidbody.velocity.z <= -0.5f)
        {
            return;
        }

        if (velocityHistory.Count > 8)
        {
            velocityHistory.Dequeue();
        }

        velocityHistory.Enqueue(myRigidbody.velocity);

    }

    public void GraspEnd()
    {
        Debug.Log("Stopped grab");
        audioSource.clip = onThrowSound;
        audioSource.Play();

        Vector3 averageVelocity = Vector3.zero;
        foreach (Vector3 velocity in velocityHistory)
        {
            Debug.Log(velocity);
            averageVelocity += velocity;
        }
        averageVelocity /= velocityHistory.Count;
        velocityHistory.Clear();

        Debug.Log("Average velocity: " + averageVelocity);
        
        // If the angle between throw and displacement is close and it is slow, speed it up!
        if (Vector3.Angle(myRigidbody.velocity, target.transform.position - transform.position) < 30f)
        {
            if (myRigidbody.velocity.sqrMagnitude < 2)
            {
                myRigidbody.velocity *= 3;
            }
            else if (myRigidbody.velocity.sqrMagnitude < 4)
            {
                myRigidbody.velocity *= 2;
            }
        }
        else
        {
            myRigidbody.velocity = averageVelocity * 2f; // Maybe something went wrong? Take average
        }


        float timeToReset = 5f;
        if (myRigidbody.velocity.sqrMagnitude < 2)
        {
            //timeToReset = 2f;
        }
        Invoke("MissedBall", timeToReset);
    }

    private void OnTriggerEnter(Collider other)
    {
        // We hit target...
        CancelInvoke();
    }
}
