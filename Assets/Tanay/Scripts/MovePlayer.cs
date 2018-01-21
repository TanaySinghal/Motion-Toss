using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    [SerializeField]
    private Transform ball;
    
    const float moveSpeed = 1f;
    const float rotateSpeed = 100f;

    float initHeight;

    void Start()
    {
        initHeight = transform.position.y;
    }

	// Update is called once per frame
	void Update ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(0, 0, moveVertical) * moveSpeed * Time.deltaTime;

        transform.position += transform.rotation * movement;
        transform.position = new Vector3(transform.position.x, initHeight, transform.position.z);

        // Just rotate
        Vector3 rotation = new Vector3(0, moveHorizontal, 0) * rotateSpeed * Time.deltaTime;
        transform.eulerAngles += rotation;
	}
}
