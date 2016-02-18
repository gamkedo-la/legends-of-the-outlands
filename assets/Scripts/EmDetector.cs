using UnityEngine;
using System.Collections;

public class EmDetector : MonoBehaviour {
	public static bool EmOnToaster = false;

	void Start() {
		EmOnToaster = false;
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name.Contains("Em")) {
			EmOnToaster = true;
			Debug.Log("Em on Toaster");
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.name.Contains("Em")) {
			EmOnToaster = false;
			Debug.Log("Em off Toaster");
		}
    }
}
