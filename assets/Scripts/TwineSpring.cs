using UnityEngine;
using System.Collections;

public class TwineSpring : Photon.MonoBehaviour {

	public string removeThisNameOnEntry;
	public Animation[] animOnEntry;
	bool bandAttachedYet = false;

	void Start () {
		for(int i = 0; i < animOnEntry.Length; i++) {
			animOnEntry[i].enabled = false;
		}
	}

	[PunRPC]
	void AttachPin() {
		if(bandAttachedYet) {
			return;
		}
		bandAttachedYet = true;
		GameObject pinUsed = GameObject.Find(removeThisNameOnEntry);
		if(pinUsed) {
			pinUsed.SetActive(false);
		}
		for(int i = 0; i < animOnEntry.Length; i++) {
			animOnEntry[i].enabled = true;
		}

		this.enabled = false;
	}
	
	void OnTriggerEnter (Collider entered) {
		Debug.Log(entered.name);
		if(entered.name.Contains(removeThisNameOnEntry)) {
			removeThisNameOnEntry = entered.name; // exact match for removal soon
			if(photonView == null) {
				AttachPin();
			} else {
				photonView.RPC("AttachPin", PhotonTargets.All);
			}
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(bandAttachedYet);
		}
		else
		{
			// Network player, receive data
			bandAttachedYet = (bool) stream.ReceiveNext();
		}
	}
}
