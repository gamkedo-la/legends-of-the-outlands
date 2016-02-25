using UnityEngine;
using System.Collections;

public class SubtleDrift : MonoBehaviour {
	Quaternion baseRot;
	Vector3 basePos;
	// Use this for initialization
	void Start () {
		baseRot = transform.rotation;
		basePos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = baseRot *
			Quaternion.AngleAxis( Mathf.Cos (Time.time*0.4f) * 0.7f, Vector3.up) *
				Quaternion.AngleAxis( Mathf.Cos (Time.time*0.15f) * 1.3f, Vector3.right);

		transform.position = basePos +
			Vector3.up * Mathf.Cos (Time.time*0.3f) * 0.3f +
			Vector3.right * Mathf.Cos (Time.time*0.07f) * 29.1f;
	}
}
