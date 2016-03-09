using UnityEngine;
using System.Collections;

public class YankToHereAtStart : MonoBehaviour {
	public Transform moveObj;
	// Use this for initialization
	void Start () {
		StartCoroutine(MomentLater());
	}

	IEnumerator MomentLater() {
		yield return new WaitForSeconds(0.1f);
		moveObj.transform.position = transform.position;
	}
}
