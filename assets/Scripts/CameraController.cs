using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	public float cameraUpMod = 25.0f;
	public float cameraRightMod = -30.0f;
	public float cameraForwardMod = 15.0f;
	
	Vector3 moveCamTo;
	// Use this for initialization
	void Start () {
		moveCamTo = transform.position - transform.forward * cameraForwardMod + Vector3.up * cameraUpMod;
	}
	
	// Update is called once per frame
	void Update () {

		moveCamTo = transform.position - transform.forward * cameraForwardMod + Vector3.up * cameraUpMod;
		Camera.main.transform.LookAt (transform.position + transform.forward * cameraForwardMod);

		
		float bias = 0.92f;
		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);

	}
}
