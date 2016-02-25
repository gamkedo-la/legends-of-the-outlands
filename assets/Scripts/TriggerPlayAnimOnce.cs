using UnityEngine;
using System.Collections;

public class TriggerPlayAnimOnce : MonoBehaviour {
	public GameObject playAnimOn;

	bool playedYet = false;

	public void FireAnim() {
		if(playedYet == false) {
			playedYet = true;
			Animator playWhichAnim = playAnimOn.GetComponent<Animator>();
			playWhichAnim.enabled = true;
		}
	}

	void OnTriggerEnter (Collider whoEnters) {
		if(whoEnters.tag == "Player") {
			FireAnim();
		}
	}
}
