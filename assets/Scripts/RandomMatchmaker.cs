using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon;

public class RandomMatchmaker : Photon.PunBehaviour {

	public Transform spawnPoint;
	public bool singlePlayer = false;
	public Toggle networkCheckbox;

	public RatID defaultPlayerRat = RatID.Rawl;

	string whichRat;
	string AIRat;
	RoomOptions roomOptions;

	public static RandomMatchmaker instance;

	public override void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom ();
	}

	public void ToggleSPBasedOnCheckbox() {
		singlePlayer = (networkCheckbox.isOn == false);
	}

	void CreateAIRat(Transform leader){
		GameObject AIMonster = Instantiate(Resources.Load(AIRat), spawnPoint.position, Quaternion.identity) as GameObject;
		Companion_AI AIController = AIMonster.GetComponent<Companion_AI> ();
		AIController.enabled = true;
		CarryingController AICarry = AIMonster.GetComponent<CarryingController> ();
		AICarry.enabled = false; // block it from getting player input messages
		AIController.SetLeaderRat (leader);
	}

	void JoinRoomAndCreateCharacter(){
		if (PhotonNetwork.playerList.Length == 1) {
			string EmPrefab = "Prefabs/Em";
			string RawlPrefab = "Prefabs/Rawl";
				
			switch(defaultPlayerRat) {
			case RatID.Rawl:
				whichRat = RawlPrefab;
				break;
			case RatID.Em:
				whichRat = EmPrefab;
				break;
			case RatID.Either:
			default:
				whichRat = (Random.Range (0.0f, 1.0f) < 0.5f ? EmPrefab : RawlPrefab);
				break;
			}


			if (whichRat == EmPrefab) {
				PhotonNetwork.playerName = "Em";
				AIRat = RawlPrefab;
			} else {
				PhotonNetwork.playerName = "Rawl";
				AIRat = EmPrefab;
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
		CarryingController carryController = monster.GetComponent<CarryingController> ();
		carryController.enabled = true;

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
		instance = this;

		ToggleSPBasedOnCheckbox();
		if (singlePlayer == false) {
			roomOptions = new RoomOptions () { isVisible = true, maxPlayers = 2 };
			PhotonNetwork.ConnectUsingSettings ("0.1");
		} else {
			PhotonNetwork.offlineMode = true;
			PhotonNetwork.JoinRandomRoom ();
		}
	}

	void OnGUI(){
		if(PhotonNetwork.connectionStateDetailed != PeerState.Joined) {
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		} else if(RandomMatchmaker.instance && RandomMatchmaker.instance.singlePlayer == false &&
			PhotonNetwork.playerList.Length==1) {
			GUILayout.Label("Connected but waiting for another player to start online");
		}
	}
}
