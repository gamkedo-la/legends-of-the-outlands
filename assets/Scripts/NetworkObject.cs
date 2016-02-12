using UnityEngine;
using Photon;

public class NetworkObject : Photon.MonoBehaviour {

	RandomMatchmaker matchSettings;
	Vector3 correctObjectPos;
	Quaternion correctObjectRot;

	void Start(){
		matchSettings = GameObject.Find("ScriptsManager").GetComponent<RandomMatchmaker> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (matchSettings.singlePlayer == false) {
						
				transform.position = Vector3.Lerp(transform.position, this.correctObjectPos, Time.deltaTime * 5);
				transform.rotation = Quaternion.Lerp(transform.rotation, this.correctObjectRot, Time.deltaTime * 5);
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
				this.correctObjectPos = (Vector3)stream.ReceiveNext();
				this.correctObjectRot = (Quaternion)stream.ReceiveNext();
			}
		}
	}
}
