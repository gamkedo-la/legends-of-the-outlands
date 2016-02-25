using UnityEngine;
using System.Collections;

public class ToggleKinematicAtStart : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().isKinematic = !GetComponent<Rigidbody>().isKinematic;
		if(GetComponent<Rigidbody>().isKinematic) {
			Destroy(this);
		}
	}
}
