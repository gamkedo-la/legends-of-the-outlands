using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	public float cameraUpMod = 25.0f;
	public float cameraForwardMod = 15.0f;
	public float cameraUpTiltOffset = 5.0f;
	public float bias = 0.92f;
	public float horRotateSpeed = 1.0f;
	public float virRotateSpeed = 1.0f;

	bool freeCamEnabled;

	float cameraMovementTime = 1.0f;
	float lastMouseMovement;
	Vector3 moveCamTo;
	Vector3 previousMousePos;

	// Use this for initialization
	void Start () {
		moveCamTo = transform.position - transform.forward * cameraForwardMod + Vector3.up * cameraUpMod;
		freeCamEnabled = false;
		previousMousePos = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis ("Mouse X") < 0.0f && Input.mousePosition != previousMousePos) {
			Camera.main.transform.Rotate(0, -1.0f * Time.deltaTime * horRotateSpeed, 0);
			lastMouseMovement = Time.time;
		} else if (Input.GetAxis ("Mouse X") > 0.0f && Input.mousePosition != previousMousePos) {
			Camera.main.transform.Rotate(0, 1.0f * Time.deltaTime * horRotateSpeed, 0);
			lastMouseMovement = Time.time;
		}
		if (Input.GetAxis ("Mouse Y") < 0.0f && Input.mousePosition != previousMousePos) {
			Camera.main.transform.Rotate(1.0f * Time.deltaTime * virRotateSpeed, 0, 0);
			lastMouseMovement = Time.time;
		} else if (Input.GetAxis ("Mouse Y") > 0.0f && Input.mousePosition != previousMousePos) {
			Camera.main.transform.Rotate(-1.0f * Time.deltaTime * virRotateSpeed, 0, 0);
			lastMouseMovement = Time.time;
		}

		if (Time.time - lastMouseMovement < cameraMovementTime) {
			freeCamEnabled = true;
		} else {
			freeCamEnabled = false;
		}

		if (freeCamEnabled == false){
			moveCamTo = transform.position - transform.forward * cameraForwardMod + Vector3.up * cameraUpMod;
			Camera.main.transform.LookAt (transform.position + Vector3.up * cameraUpTiltOffset);
			Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
		}


	}
}
