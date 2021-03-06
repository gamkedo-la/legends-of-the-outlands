﻿using UnityEngine;
using System.Collections;

public class TitleScreenSwitcher : MonoBehaviour {
	public Transform spawnPointForCameraWarp;
	public RandomMatchmaker gameStarter;
	public GameObject titleScreenCanvas;

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
		if (Input.GetKeyDown(KeyCode.Space) && Camera.main.transform.parent != null){
			GameStart();
			titleScreenCanvas.SetActive(false);
		}
	}
	
	public void GameStart () {
		Camera.main.transform.parent = null; // release it
		Camera.main.transform.position = spawnPointForCameraWarp.position;
		Camera.main.transform.rotation = spawnPointForCameraWarp.rotation;
		SongSwitcher songControl = Camera.main.GetComponent<SongSwitcher>();
		if(songControl) {
			songControl.StartGameMusic();
		}
		gameStarter.enabled = true;
	}
}
