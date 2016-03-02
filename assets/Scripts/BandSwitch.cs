using UnityEngine;
using System.Collections;

public class BandSwitch : Photon.MonoBehaviour {

	public GameObject removeThisOnEntry;
	public GameObject appearThisOnEntry;
	public GameObject makeCarryable;
	bool bandAttachedYet = false;

	void Start () {
		removeThisOnEntry.SetActive(true);
		appearThisOnEntry.SetActive(false);
	}

	[PunRPC]
	void AttachBand() {
		if(bandAttachedYet) {
			return;
		}
		bandAttachedYet = true;
		removeThisOnEntry.SetActive(false);
		appearThisOnEntry.SetActive(true);
		makeCarryable.tag = "Carryable";
		makeCarryable.layer = LayerMask.NameToLayer("Carryable");
		Rigidbody rb = makeCarryable.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		this.enabled = false;
	}
	
	void OnTriggerEnter (Collider entered) {
		Debug.Log(entered.name);
		if(entered.name == removeThisOnEntry.name) {
			if(photonView == null) {
				AttachBand();
			} else {
				photonView.RPC("AttachBand", PhotonTargets.All);
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
