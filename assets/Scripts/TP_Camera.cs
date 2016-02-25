using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour {

	public static TP_Camera Instance;

	public Transform TargetLookAt;
	public float Distance = 5.0f;
	public float DistanceMin = 0.55f;
	public float DistanceMax = 0.55f;
	public float DistanceSmooth = 0.1f;
	public float X_MouseSensitivity = 2.5f;
	public float Y_MouseSensitivity = 0.5f;
	public float MouseWheelSensitivity = 5.0f;
	public float Y_MinLimit = -15.0f;
	public float Y_MaxLimit = 20.0f;
	public float X_Smooth = 0.2f;
	public float Y_Smooth = 0.2f;

	public float mouseX = 0.0f;
	float mouseY = 0.0f;
	float velX = 0.0f;
	float velY = 0.0f;
	float velZ = 0.0f;
	float startDistance = 0.0f;
	float desiredDistance = 0.0f;
	Vector3 desiredPosition = Vector3.zero;
	Vector3 position = Vector3.zero;
	float velDistance = 0.0f;


	void Awake(){
		Instance = this;
	}


	void Start () {
		Distance = Mathf.Clamp (Distance, DistanceMin, DistanceMax);
		startDistance = Distance;
		Reset ();
	}

	void LateUpdate () {
		if (TargetLookAt == null) {
			return;
		}
		HandlePlayerInput ();

		CalculateDesiredPosition ();

		UpdatePosition ();
	}

	void HandlePlayerInput(){

		var deadZone = 0.01f;

		mouseX += Input.GetAxis ("Mouse X") * X_MouseSensitivity;
		mouseY -= Input.GetAxis ("Mouse Y") * Y_MouseSensitivity;

		mouseY = Helper.ClampAngle (mouseY, Y_MinLimit, Y_MaxLimit);

		if (Input.GetAxis ("Mouse ScrollWheel") < -deadZone || Input.GetAxis ("Mouse ScrollWheel") > deadZone) {
			desiredDistance = Mathf.Clamp (Distance - Input.GetAxis ("Mouse ScrollWheel") * MouseWheelSensitivity, DistanceMin, DistanceMax);
		}
	}

	void CalculateDesiredPosition(){
		Distance = Mathf.SmoothDamp (Distance, desiredDistance, ref velDistance, DistanceSmooth);

		desiredPosition = CalculatePosition (mouseY, mouseX, Distance);
	}

	Vector3 CalculatePosition(float rotationX, float rotationY, float distance){
		Vector3 direction = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (rotationX, rotationY, 0);
		RaycastHit rhInfo;
		if(Physics.Raycast(TargetLookAt.position, rotation * direction, out rhInfo, distance)) {
			float newDist = Vector3.Distance(rhInfo.point, TargetLookAt.position);
			direction = direction.normalized * newDist * 0.6f;
			return TargetLookAt.position + rotation * direction;
		} else {
			return TargetLookAt.position + rotation * direction;
		}
	}

	void UpdatePosition(){
		var posX = Mathf.SmoothDamp (position.x, desiredPosition.x, ref velX, X_Smooth);
		var posY = Mathf.SmoothDamp (position.y, desiredPosition.y, ref velY, Y_Smooth);
		var posZ = Mathf.SmoothDamp (position.z, desiredPosition.z, ref velZ, X_Smooth);
		position = new Vector3 (posX, posY, posZ);

		transform.position = position;

		/*transform.rotation = Quaternion.Slerp( transform.rotation,
			Quaternion.LookRotation( TargetLookAt.position - transform.position), 10.0f * Time.deltaTime);*/
		transform.LookAt (TargetLookAt.position);
	}

	public void Reset(){
		mouseX = 0.0f;
		mouseY = 10.0f;
		Distance = startDistance;
		desiredDistance = Distance;
	}

	public static void UseExistingOrCreateNewMainCamera(){
		GameObject tempCamera;
		GameObject targetLookAt;
		TP_Camera myCamera;

		if (Camera.main != null) {
			tempCamera = Camera.main.gameObject;
		} else {
			tempCamera = new GameObject ("Main Camera");
			tempCamera.AddComponent <Camera>();
			tempCamera.tag = "Main Camera";
		}

		tempCamera.AddComponent <TP_Camera>();
		myCamera = tempCamera.GetComponent ("TP_Camera") as TP_Camera;

		targetLookAt = GameObject.Find ("targetLookAt") as GameObject;

		if (targetLookAt == null) {
			targetLookAt = new GameObject ("targetLookAt");
			targetLookAt.transform.position = Vector3.zero;
		}

		myCamera.TargetLookAt = targetLookAt.transform;
	}
}
