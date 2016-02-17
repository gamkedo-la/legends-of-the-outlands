using UnityEngine;
using System.Collections;
using Photon;

public class RandomMatchmaker : Photon.PunBehaviour {

	public Transform spawnPoint;
	public bool singlePlayer = false;

	string whichRat;
	string AIRat;
	RoomOptions roomOptions;

	public override void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom ();
	}

	void CreateAIRat(Transform leader){
		Vector3 AIoffset = Random.onUnitSphere;
		AIoffset.y = 0.0f;
		AIoffset.Normalize();
		GameObject AIMonster = Instantiate(Resources.Load(AIRat), spawnPoint.position+AIoffset, Quaternion.identity) as GameObject;
		Companion_AI AIController = AIMonster.GetComponent<Companion_AI> ();
		AIController.enabled = true;
		AIController.SetLeaderRat (leader);
	}

	void JoinRoomAndCreateCharacter(){
		if (PhotonNetwork.playerList.Length == 1) {
			whichRat = (Random.Range (0.0f, 1.0f) < 0.5f ? "Prefabs/Em" : "Prefabs/Rawl");
			if (whichRat == "Prefabs/Em") {
				PhotonNetwork.playerName = "Em";
				AIRat = "Prefabs/Rawl";
			} else {
				PhotonNetwork.playerName = "Rawl";
				AIRat = "Prefabs/Em";
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

		if (singlePlayer) {
			CreateAIRat (monster.transform);
		}
	}

	public override void OnJoinedRoom(){
		JoinRoomAndCreateCharacter ();
	}

	void OnPhotonRandomJoinFailed(){
		Debug.Log ("First!");
		PhotonNetwork.CreateRoom (null, roomOptions, TypedLobby.Default);

	}

	// Use this for initialization
	void Start () {
		if (singlePlayer == false) {
			roomOptions = new RoomOptions () { isVisible = true, maxPlayers = 2 };
			PhotonNetwork.ConnectUsingSettings ("0.1");
		} else {
			PhotonNetwork.offlineMode = true;
			PhotonNetwork.JoinRandomRoom ();
		}
	}

	void OnGUI(){
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}
