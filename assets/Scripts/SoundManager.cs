using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance;

	void Awake() {
		if(instance) {
			Destroy(instance.gameObject);
		}
		instance = this;
	}

	public void PlayRandClipOn(AudioClip[] clipList, Vector3 pos, float atVol = 1.0f,
		Transform attachToParent = null){
		AudioClip tempClip = clipList [Random.Range (0, clipList.Length)];
		PlayClipOn (tempClip, pos, atVol, attachToParent);
	}

	public void PlayClipOn(AudioClip clip, Vector3 pos, float atVol = 1.0f,
		Transform attachToParent = null) {
		GameObject tempGO = new GameObject("TempAudio"); // create the temp object
		tempGO.transform.position = pos; // set its position
		if(attachToParent != null) {
			tempGO.transform.parent = attachToParent.transform;
		}
		AudioSource aSource = tempGO.AddComponent<AudioSource>() as AudioSource; // add an audio source
		aSource.clip = clip; // define the clip
		aSource.volume = atVol;
		aSource.pitch = Random.Range(0.9f,1.1f);
		// set other aSource properties here, if desired
		aSource.Play(); // start the sound
		Destroy(tempGO, clip.length/aSource.pitch); // destroy object after clip duration
	}
}