using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionControlDisplay : MonoBehaviour {

	public Text actionText;
	public float fadeSpeed = 5f;
	public bool entrance;
	public GameObject canvasPlayerInstruction;

	void Start () {

		actionText = canvasPlayerInstruction.GetComponentInChildren<Text> ();
		actionText.color = Color.clear;

	}

	void Update () {

		ColorChange();
	}

	void OnTriggerEnter (Collider col){
		if (col.gameObject.tag == "Player") {
			entrance = true;
		}

	}

	void OnTriggerExit (Collider col){
		if (col.gameObject.tag == "Player") {
			entrance = false;

		}

	}

	void ColorChange() {
		if (entrance) {
			actionText.color = Color.Lerp (actionText.color, Color.white, fadeSpeed * Time.deltaTime);
		}

		if (entrance) {
			actionText.color = Color.Lerp (actionText.color, Color.clear, fadeSpeed * Time.deltaTime);
		}
	}
}
			
