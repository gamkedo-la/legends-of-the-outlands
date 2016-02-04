using UnityEngine;
using System.Collections;

public class ToasterShootWhatsInside : MonoBehaviour {
	public Transform toastBody;
	Vector3 toastBodyLoadedOffset;
	bool isLaunchedYet = false;
	bool isLoaded = false;

	void Start() {
		toastBodyLoadedOffset = toastBody.transform.position - transform.position;
	}

	void Update() {
		if(isLaunchedYet == false) {
			toastBody.transform.position = transform.position + toastBodyLoadedOffset;
		}
	}

	// Update is called once per frame
	public void ShootToasterLoaded () {
		isLoaded = true;
	}

	public void ShootToasterContents () {
		if(isLoaded) {
			Debug.Log("it's toast time!");
			Animator toastAnim = toastBody.GetComponent<Animator>();
			if(toastAnim.enabled == false) {
				toastAnim.enabled = true;
			}
			isLaunchedYet = true;
			isLoaded = false;
		}
	}
}
