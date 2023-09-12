using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour {

    public float speed = 1.0f;

    public Vector3 startPos;
    public Vector3 endPos;

	// Use this for initialization
	void Start () {

        startPos = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {

        float step = speed * Time.deltaTime;

        // Start moving
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);

        // If at stop point move back to beginning
        if (transform.position == endPos)
            transform.position = startPos;
	}
}
