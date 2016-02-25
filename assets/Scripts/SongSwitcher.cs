using UnityEngine;
using System.Collections;

public class SongSwitcher : MonoBehaviour {
	public AudioClip tunnelSong;
	public AudioClip transitionSong;
	public AudioClip kitchenSong;

	enum SongMode {TUNNEL,TRANSITON, KITCHEN, NONE_YET};
	SongMode songNow = SongMode.NONE_YET;

	AudioSource jukeBox;

	public Transform groundPlaneForY;

	// Use this for initialization
	void Start () {
		jukeBox = GetComponent<AudioSource>();
	}

	void CheckAndPlay(SongMode songEnum, AudioClip songFile) {
		if(songNow != songEnum) {
			songNow = songEnum;
			jukeBox.clip = songFile;
			jukeBox.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < groundPlaneForY.position.y) {
			CheckAndPlay(SongMode.KITCHEN, kitchenSong);
		} else {
			CheckAndPlay(SongMode.TUNNEL, tunnelSong);
		}
	}
}
