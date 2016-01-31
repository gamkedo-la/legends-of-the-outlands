using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float rotateSpeed = 50.0f;
	public float catchTime = 0.25f;
	public AudioClip squeak;
	public AudioClip chatter;

	float lastClickTime;


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

		if (Input.GetMouseButtonDown (1)) {
			if (Time.time - lastClickTime < catchTime) {
				SoundManager.instance.PlayNamedClipOn (chatter, transform.position, gameObject.name, "chatter", 1.0f, transform);	
			} else {
				SoundManager.instance.PlayNamedClipOn (squeak, transform.position, gameObject.name, "squeak", 1.0f, transform);	
			}
			lastClickTime = Time.time;
		}

		Movement ();
	}
}
