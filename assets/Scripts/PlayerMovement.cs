using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float rotateSpeed = 50.0f;


	void Movement(){
		transform.position += transform.forward * (Time.deltaTime * movementSpeed * Input.GetAxis ("Vertical"));
		transform.Rotate(0, Input.GetAxis ("Horizontal") * Time.deltaTime * rotateSpeed, 0);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			transform.rotation = Quaternion.identity;
		}

		Movement ();
	}
}
