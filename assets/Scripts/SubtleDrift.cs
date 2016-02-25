using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubtleDrift : MonoBehaviour {
	Quaternion baseRot;
	Vector3 basePos;
	float startTime;
	public float motionRange = 29.1f;
	bool rangeMoving = true;

	public Text fadeMessage;

	public bool tipUp = false;

	// Use this for initialization
	void Start () {
		baseRot = transform.rotation;
		basePos = transform.position;
		startTime = Time.time;
		if(fadeMessage) {
			fadeMessage.color = Color.clear;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float relTime = Time.time - startTime;
		if(tipUp && rangeMoving == false) {
			if(transform.up.y > 0.0f) {
				transform.Rotate(Vector3.right, -Time.deltaTime * 15.0f);
			} else if(fadeMessage) {
				Color tempCol = fadeMessage.color;
				if(tempCol.a < 1.0f) {
					tempCol.a += Time.deltaTime * 0.5f;
					if(tempCol.a > 1.0f) {
						tempCol.a = 1.0f;
					}
					fadeMessage.color = tempCol;
				}
			}
		} else {
			transform.rotation = baseRot *
			Quaternion.AngleAxis(Mathf.Cos(relTime * 0.4f) * 0.7f, Vector3.up) *
			Quaternion.AngleAxis(Mathf.Cos(relTime * 0.15f) * 1.3f, Vector3.right);
		}

		float rangePace = 0.07f;
		transform.position = basePos +
			Vector3.up * Mathf.Cos (relTime*0.3f) * 0.3f +
			(rangeMoving ? Vector3.right * Mathf.Cos (relTime*rangePace) * motionRange : Vector3.right * -motionRange );

		if(relTime * rangePace >= Mathf.PI) {
			rangeMoving = false;
		}
	}
}
