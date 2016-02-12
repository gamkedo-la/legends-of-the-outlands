using UnityEngine;
using Photon;

public class NetworkCharacter : Photon.MonoBehaviour {

	RandomMatchmaker matchSettings;
	Vector3 correctPlayerPos;
	Quaternion correctPlayerRot;

	void Start(){
		matchSettings = GameObject.Find("ScriptsManager").GetComponent<RandomMatchmaker> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (matchSettings.singlePlayer == false) {
			if (photonView.isMine == false)
			{
				transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
				transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (matchSettings.singlePlayer == false) {
			if (stream.isWriting)
			{
				// We own this player: send the others our data
				stream.SendNext(transform.position);
				stream.SendNext(transform.rotation);

			}
			else
			{
				// Network player, receive data
				this.correctPlayerPos = (Vector3)stream.ReceiveNext();
				this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			}
		}
	}
}
