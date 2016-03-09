using UnityEngine;
using System.Collections;

public class ScaleDetectWeights : MonoBehaviour {
	public ScalesAnimatorScript masterToReportWeightTo;
	public int weightSide;

	void OnTriggerEnter(Collider other) {
		Rigidbody rb = other.GetComponent<Rigidbody>();
		if(rb == null) {
			return;
		}
		masterToReportWeightTo.WeightDelta(rb.mass, weightSide);
		Debug.Log(name+" now has "+other.name+" on it.");
	}
	void OnTriggerExit(Collider other) {
		Rigidbody rb = other.GetComponent<Rigidbody>();
		if(rb == null) {
			return;
		}
		masterToReportWeightTo.WeightDelta(-rb.mass, weightSide);
		Debug.Log(name+" no longer has "+other.name+" on it.");
	}
}
