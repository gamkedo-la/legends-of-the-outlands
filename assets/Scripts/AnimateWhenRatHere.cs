using UnityEngine;
using System.Collections;

public class AnimateWhenRatHere : MonoBehaviour {
	public Animator someAnim;

	void OnTriggerEnter (Collider other) {
		someAnim.SetBool("WeighedDown", true);
	}

	void OnTriggerExit (Collider other) {
		someAnim.SetBool("WeighedDown", false);
	}
}
