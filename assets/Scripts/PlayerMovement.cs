using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 5.0f;


	void Movement(){
		transform.position = transform.position + Vector3.forward * (Time.deltaTime * movementSpeed * Input.GetAxis ("Vertical"));
		transform.position = transform.position + Vector3.right * (Time.deltaTime * movementSpeed * Input.GetAxis ("Horizontal"));
	}
	// Update is called once per frame
	void Update () {
		Movement ();
	}
}
