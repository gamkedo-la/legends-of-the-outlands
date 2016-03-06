using UnityEngine;
using System.Collections;

public class Companion_AI : MonoBehaviour {


	public float moveSpeed = 3.0f;
	public float rotationSpeed = 3.0f;
	public float minDistance = 4.0f;

	Transform leader;
	float distToTarget;
	bool moveTowards = false;

	public void SetLeaderRat(Transform transform){
		leader = transform;
	}

	void TeleportToLeader(){
		Debug.Log ("In TTL");
//		Vector3 tempPos = leader.position -= transform.forward * minDistance;
		transform.position = leader.position + transform.up * 3.0f;;
	}

	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (leader.position - transform.position), rotationSpeed * Time.deltaTime);

		distToTarget = Vector3.Distance (transform.position, leader.position);
		if (distToTarget < minDistance) {
			moveTowards = false;
		} else if (distToTarget > 8.0f) {
			Debug.Log ("Distance to target is greater than 8.0f");
			TeleportToLeader ();
		} else {
			moveTowards = true;
		}

		if (moveTowards) {
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
		}
	}
}
