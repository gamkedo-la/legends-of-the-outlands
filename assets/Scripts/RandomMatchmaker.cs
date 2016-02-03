using UnityEngine;
using System.Collections;
using Photon;

public class RandomMatchmaker : Photon.PunBehaviour {

	public Transform spawnPoint;

	string whichRat;
	RoomOptions roomOptions;

	public override void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom ();
	}

	public override void OnJoinedRoom(){
		if (PhotonNetwork.playerList.Length == 1) {
			whichRat = (Random.Range (0.0f, 1.0f) < 0.5f ? "Prefabs/Em" : "Prefabs/Rawl");
			if (whichRat == "Prefabs/Em") {
				PhotonNetwork.playerName = "Em";
			} else {
				PhotonNetwork.playerName = "Rawl";
			}
		} else {
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
				if (PhotonNetwork.playerList [i].isMasterClient) {
					if (PhotonNetwork.playerList [i].name == "Em") {
						whichRat = "Prefabs/Rawl";
						PhotonNetwork.playerName = "Rawl";
					} else {
						whichRat = "Prefabs/Em";
						PhotonNetwork.playerName = "Em";
					}
				}
			}
		}
		GameObject monster = PhotonNetwork.Instantiate(whichRat , spawnPoint.position, Quaternion.identity, 0);
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
