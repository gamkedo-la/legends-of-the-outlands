using UnityEngine;
using System.Collections;

public class TriggerPlayAnimOnce : MonoBehaviour {
	public GameObject playAnimOn;

	bool playedYet = false;

	void OnTriggerEnter (Collider whoEnters) {
		if(playedYet == false && whoEnters.tag == "Player") {
			playedYet = true;
			Animator playWhichAnim = playAnimOn.GetComponent<Animator>();
			playWhichAnim.enabled = true;
		}
	}
}
