using UnityEngine;
using System.Collections;

public class TeleportBumper : MonoBehaviour {
	void OnTriggerEnter(Collider enteredThing) {
		enteredThing.transform.position = transform.GetChild(0).transform.position;
		enteredThing.transform.rotation = transform.GetChild(0).transform.rotation;
		RenderSettings.fog = false;
	}
}
