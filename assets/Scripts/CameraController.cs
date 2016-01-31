using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	public float cameraUpMod = 25.0f;
	public float cameraForwardMod = 15.0f;
	public float cameraUpTiltOffset = 5.0f;
	public float bias = 0.92f;
	
	Vector3 moveCamTo;
	// Use this for initialization
	void Start () {
		moveCamTo = transform.position - transform.forward * cameraForwardMod + Vector3.up * cameraUpMod;
	}
	
	// Update is called once per frame
	void Update () {

		moveCamTo = transform.position - transform.forward * cameraForwardMod + Vector3.up * cameraUpMod;
		Camera.main.transform.LookAt (transform.position + Vector3.up * cameraUpTiltOffset);


		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);

	}
}
