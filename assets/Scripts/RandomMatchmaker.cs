using UnityEngine;
using System.Collections;
using Photon;

public class RandomMatchmaker : Photon.PunBehaviour {

	RoomOptions roomOptions;

	public override void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom ();
	}

	public override void OnJoinedRoom(){
		GameObject monster = PhotonNetwork.Instantiate("Prefabs/Em", Vector3.zero, Quaternion.identity, 0);
		PlayerMovement controller = monster.GetComponent<PlayerMovement> ();
		controller.enabled = true;
		CameraController playerCam = monster.GetComponent<CameraController> ();
		playerCam.enabled = true;
	}

	void OnPhotonRandomJoinFailed(){
		Debug.Log ("First!");
		PhotonNetwork.CreateRoom (null, roomOptions, TypedLobby.Default);


	}

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings ("0.1");
		roomOptions = new RoomOptions() {isVisible = true, maxPlayers = 2};
	}

	void OnGUI(){
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	// Update is called once per frame
	void Update () {
	
	}
}
