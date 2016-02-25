using UnityEngine;
using System.Collections;

public class BandSwitch : MonoBehaviour {

	public GameObject removeThisOnEntry;
	public GameObject appearThisOnEntry;
	public GameObject makeCarryable;

	void Start () {
		removeThisOnEntry.SetActive(true);
		appearThisOnEntry.SetActive(false);
	}
	
	void OnTriggerEnter (Collider entered) {
		Debug.Log(entered.name);
		if(entered.name == removeThisOnEntry.name) {
			removeThisOnEntry.SetActive(false);
			appearThisOnEntry.SetActive(true);
			makeCarryable.tag = "Carryable";
			makeCarryable.layer = LayerMask.NameToLayer("Carryable");
			Rigidbody rb = makeCarryable.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			this.enabled = false;
		}
	}
}
