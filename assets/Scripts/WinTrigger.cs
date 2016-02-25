using UnityEngine;
using System.Collections;

public class WinTrigger : MonoBehaviour {
	public WinScreenSwitcher winCam;

	void OnTriggerEnter (Collider other) {
		if(other.tag == "Player") {
			Debug.Log("WIN!");

			TP_Camera thirdPerCam = Camera.main.GetComponent<TP_Camera>();
			if(thirdPerCam) {
				thirdPerCam.enabled = false;
			}

			GameObject [] bothRats = GameObject.FindGameObjectsWithTag("Player");
			for(int i = 0; i < bothRats.Length; i++) {
				bothRats[i].SetActive(false);
			}

			winCam.gameObject.SetActive(true);
		}
	}
}
