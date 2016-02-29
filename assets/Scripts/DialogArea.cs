using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public enum RatID {Em,Rawl,Either,None};

[Serializable]
public class SpeakMoment
{
	public RatID whoSays;
	public string caption;
	public AudioClip statementAudio;
}


public class DialogArea : MonoBehaviour {
	const int NOT_YET_SPOKEN = -1;
	const int ALREADY_SPOKEN = -2;

	public float timeBetweenStatements = 0.4f;
	public SpeakMoment[] linesHere;
	int spokenLine = NOT_YET_SPOKEN;

	public string showHelpAfter = "";

	void AdvanceSpokenLine() {
		spokenLine++;
		if(spokenLine >= linesHere.Length) {
			Debug.Log("Dialog done!");
			if (showHelpAfter.Length > 1) {
				HelpMessage.instance.setMessage (showHelpAfter);
			}
			spokenLine = ALREADY_SPOKEN; // block from repeat
		} else {
			DoCurrentDialogLine();
		}
	}

	private IEnumerator AdvanceLineAfterDelay(float audioLength)
	{
		yield return new WaitForSeconds(audioLength);
		AdvanceSpokenLine();
	}

	private void DoCurrentDialogLine() {
		// to support captioning just smash this same text out to the .text value on a Text UI element atop Canvas
		Debug.Log(linesHere[spokenLine].whoSays + ": " + linesHere[spokenLine].caption);

		if(linesHere[spokenLine].statementAudio != null) {
			Vector3 soundFrom;
			GameObject desiredRat;

			if(linesHere[spokenLine].whoSays == RatID.Em) {
				desiredRat = GameObject.Find("Em(Clone)");
			} else {
				desiredRat = GameObject.Find("Rawl(Clone)");
			}
			// if the desired rat is in the scene play sound from them, otherwise default to on camera for backup
			if(desiredRat != null) {
				soundFrom = desiredRat.transform.position;
			} else {
				soundFrom = Camera.main.transform.position;
			}
			AudioSource.PlayClipAtPoint(linesHere[spokenLine].statementAudio, soundFrom);
			StartCoroutine( AdvanceLineAfterDelay(linesHere[spokenLine].statementAudio.length + timeBetweenStatements) );
		} else {
			StartCoroutine( AdvanceLineAfterDelay(1.0f) ); // just so it isn't instant when there's audio missing
		}
	}

	void OnTriggerEnter (Collider whoEnters) {
		if(whoEnters.tag == "Player" && spokenLine == NOT_YET_SPOKEN) { // hasn't yet been kicked off?
			Debug.Log("dialog audio kicked off by" + whoEnters);
			spokenLine++;
			DoCurrentDialogLine();
		}
	}
}
