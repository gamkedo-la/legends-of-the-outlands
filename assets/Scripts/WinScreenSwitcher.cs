using UnityEngine;
using System.Collections;

public class WinScreenSwitcher : MonoBehaviour {
	// Use this for initialization
	void Start () {
		Camera.main.transform.parent = transform;
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.transform.localRotation = Quaternion.identity;
	}

	public void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}
	
	public void ResetScene () {
		Application.LoadLevel(Application.loadedLevel);
	}
}
