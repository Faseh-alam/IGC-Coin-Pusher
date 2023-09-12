using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

    public float rotationSpeed = 1.0f;
    public Vector3 spinDirection;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.RotateAround(GetComponent<BoxCollider>().bounds.center, spinDirection, rotationSpeed * Time.deltaTime);
	}
}