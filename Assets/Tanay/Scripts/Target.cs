using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public delegate void Action();
    public event Action OnHit;

    bool shouldMove;
    bool shouldDiminish;

    float speed;
    float amplitude;

    Vector3 initPos;

    Renderer r;
    [SerializeField]
    Color initColor;

    private void Awake()
    {
        r = GetComponent<Renderer>();
        Material m = r.material;
        if (m == null) Debug.LogWarning("No material!");
        initColor = r.material.color;

        shouldMove = false;
        shouldDiminish = false;
    }

    private void Start()
    {
        ChangePosition(0, 1);
    }

    private void ChangePosition(float angle, float distance)
    {
        float radians = Mathf.Deg2Rad * angle;

        float newX = Mathf.Sin(radians) * distance;
        float newZ = Mathf.Cos(radians) * distance;

        Debug.Log(distance);
        transform.position = new Vector3(newX, 0, newZ);

        transform.LookAt(Camera.main.transform);

        initPos = transform.position;

        shouldDiminish = false;
        r.material.color = initColor;
    }

    public void ChangePositionWithoutMovement(float angle, float distance)
    {
        ChangePosition(angle, distance);
        DisableMovement();
    }

    public void ChangePositionWithMovement(float angle, float distance, float _amplitude)
    {
        ChangePosition(angle, distance);
        EnableMovement(_amplitude);
    }

    private void EnableMovement(float _amplitude)
    {
        shouldMove = true;
        amplitude = _amplitude;
    }

    private void DisableMovement()
    {
        shouldMove = false;
    }

    private void Update()
    {
        //ChangePosition(angle, distance);
        if (shouldMove)
        {
            Vector3 offset = transform.rotation * Vector3.right * Mathf.Sin(Time.time) * amplitude;
            transform.position = initPos + offset;
        }

        if (shouldDiminish)
        {
            // Start disappearing
            r.material.color = Color.Lerp(r.material.color, Color.clear, 0.1f);

            if (r.material.color.a < 0.01f)
            {
                OnHit();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<AudioSource>().Play();
        shouldDiminish = true;
    }
}
