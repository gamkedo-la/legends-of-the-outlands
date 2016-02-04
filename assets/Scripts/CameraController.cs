using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	public float cameraUpMod = 25.0f;
	public float cameraForwardMod = 15.0f;
	public float cameraUpTiltOffset = 5.0f;
	public float bias = 0.92f;
	public float virRotateSpeed = 1.0f;

	float lastMouseMovement;
	Vector3 moveCamTo;
	bool lateralCamChase = false;

	// Use this for initialization
	void Start () {
		moveCamTo = transform.position - transform.forward * cameraForwardMod + Vector3.up * cameraUpMod;
	}

	void MouseCamLook(){

		if (Input.GetAxis ("Mouse Y") < 0.0f) {
			Camera.main.transform.Rotate(1.0f * Time.deltaTime * virRotateSpeed, 0, 0);
		} else if (Input.GetAxis ("Mouse Y") > 0.0f) {
			Camera.main.transform.Rotate(-1.0f * Time.deltaTime * virRotateSpeed, 0, 0);
		}

	}

	void LateUpdate () {
		lateralCamChase = (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))) > 0.1f;

		if(lateralCamChase) {
			CamPosUpdate ();
		}
	}

	void FixedUpdate () {
		if(lateralCamChase == false) {
			CamPosUpdate ();
		}
	}

	void CamPosUpdate() {
		moveCamTo = transform.position - transform.forward * cameraForwardMod + Vector3.up * cameraUpMod;
		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);    
		Camera.main.transform.LookAt(transform.position + Vector3.up * cameraUpTiltOffset);
	}
}