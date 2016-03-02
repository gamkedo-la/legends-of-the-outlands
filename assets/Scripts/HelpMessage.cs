using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelpMessage : MonoBehaviour {
	private Text helpText;
	public static HelpMessage instance;

	// Use this for initialization
	void Start () {
		instance = this;
		helpText = gameObject.GetComponent<Text> ();
	}
	
	public void setMessage (string showText){
		helpText.text = showText;
		StartCoroutine (temporaryMessage ());
	}
	IEnumerator temporaryMessage (){
		yield return new WaitForSeconds (5.0f);
		helpText.text = "";
	}
}
