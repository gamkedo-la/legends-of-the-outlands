using UnityEngine;
using System.Collections;

public class SongSwitcher : MonoBehaviour {
	public AudioClip tunnelSong;
	public AudioClip transitionSong;
	public AudioClip kitchenSong;

	enum SongMode {TUNNEL,TRANSITON, KITCHEN, NONE_YET};
	SongMode songNow = SongMode.NONE_YET;
	SongMode songNext = SongMode.NONE_YET;

	AudioSource jukeBox;
	AudioSource jukeBoxFader;
	bool blockNewEnum = false;

	public Transform groundPlaneForY;

	// Use this for initialization
	void Start () {
		jukeBox = GetComponent<AudioSource>();
		jukeBoxFader = transform.GetChild(0).GetComponent<AudioSource>();

		songNow = SongMode.TUNNEL;
		jukeBox.clip = tunnelSong;
		jukeBox.Play();

		// jukeBoxFader.clip = transitionSong;
		// jukeBoxFader.Play();
	}

	IEnumerator SongTransition() {
		blockNewEnum = true;
		jukeBox.clip = transitionSong;
		jukeBox.Play();
		yield return new WaitForSeconds(transitionSong.length);
		Debug.Log("MUSIC TRANSITION DONE");
		switch( songNow ) {
		case SongMode.TUNNEL:
			jukeBox.clip = tunnelSong;
			jukeBox.Play();
			break;
		case SongMode.KITCHEN:
			jukeBox.clip = kitchenSong;
			jukeBox.Play();
			break;
		default:
			Debug.Log("Song transition is only meant to feed into kitchen or tunnel song");
			break;
		}
		blockNewEnum = false;
	}

	void CheckAndPlay(SongMode songEnum, AudioClip songFile) {
		if(songNow != songEnum) {
			songNow = songEnum;
			if(blockNewEnum == false) {
				StartCoroutine(SongTransition());
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y > groundPlaneForY.position.y) {
			CheckAndPlay(SongMode.KITCHEN, kitchenSong);
		} else {
			CheckAndPlay(SongMode.TUNNEL, tunnelSong);
		}
	}
}
