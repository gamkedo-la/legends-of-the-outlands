using UnityEngine;
using System.Collections;

public class BandSwitch : MonoBehaviour {

	public GameObject removeThisOnEntry;
	public GameObject appearThisOnEntry;

	void Start () {
		removeThisOnEntry.SetActive(true);
		appearThisOnEntry.SetActive(false);
	}
	
	void OnTriggerEnter (Collider entered) {
		Debug.Log(entered.name);
		if(entered.name == removeThisOnEntry.name) {
			removeThisOnEntry.SetActive(false);
			appearThisOnEntry.SetActive(true);
			this.enabled = false;
		}
	}
}
