using UnityEngine;
using System.Collections;

public class SongSwitcher : MonoBehaviour {
	public AudioClip titleSong;

	public AudioClip tunnelSong;
	public AudioClip transitionSong;
	public AudioClip kitchenSong;

	enum SongMode {TUNNEL,TRANSITON, KITCHEN, TITLE, NONE_YET};
	SongMode songNow = SongMode.NONE_YET;

	AudioSource jukeBox;
	AudioSource jukeBoxFader;
	bool blockNewEnum = false;

	public Transform groundPlaneForY;

	// Use this for initialization
	void Start () {
		jukeBox = GetComponent<AudioSource>();
		jukeBoxFader = transform.GetChild(0).GetComponent<AudioSource>();

		songNow = SongMode.TITLE;
		jukeBox.clip = titleSong;
		jukeBox.Play();
		// RenderSettings.fog = false;

		// jukeBoxFader.clip = transitionSong;
		// jukeBoxFader.Play();
	}

	public void StartGameMusic() {
		songNow = SongMode.TUNNEL;
		jukeBox.clip = tunnelSong;
		jukeBox.Play();
		// RenderSettings.fog = true;
	}


	IEnumerator SongTransition() {
		Debug.Log("transitioning song");
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
			// RenderSettings.fog = (songEnum == SongMode.TUNNEL);
			songNow = songEnum;
			if(blockNewEnum == false) {
				StartCoroutine(SongTransition());
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(songNow == SongMode.TITLE) {
			return;
		}
		if(transform.position.y > groundPlaneForY.position.y) {
			CheckAndPlay(SongMode.KITCHEN, kitchenSong);
		} else {
			CheckAndPlay(SongMode.TUNNEL, tunnelSong);
		}
	}
}
