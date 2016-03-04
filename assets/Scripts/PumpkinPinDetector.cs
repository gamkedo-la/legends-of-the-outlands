using UnityEngine;
using System.Collections;

public class PumpkinPinDetector : Photon.MonoBehaviour {

	public GameObject expectingObject;
	public GameObject makeCarryable;
	public static bool isPinOnPumpking = false;

	void Start () {
	}

	[PunRPC]
	void AttachPin() {
		isPinOnPumpking = true;
		if(BandSwitch.bandAttachedYet) {
			makeCarryable.tag = "Carryable";
			makeCarryable.layer = LayerMask.NameToLayer("Carryable");
			Rigidbody rb = makeCarryable.GetComponent<Rigidbody>();
			rb.isKinematic = true;
		}
	}

	[PunRPC]
	void RemovePin() {
		/*isPinOnPumpking = false;
		makeCarryable.tag = "Untagged";
		makeCarryable.layer = LayerMask.NameToLayer("Default");
		Rigidbody rb = makeCarryable.GetComponent<Rigidbody>();
		rb.isKinematic = false;*/
	}

	void OnTriggerEnter (Collider entered) {
		if(entered.name == expectingObject.name) {
			if(photonView == null) {
				AttachPin();
			} else {
				photonView.RPC("AttachPin", PhotonTargets.All);
			}
		}
	}

	void OnTriggerExit (Collider entered) {
		if(entered.name == expectingObject.name) {
			if(photonView == null) {
				RemovePin();
			} else {
				photonView.RPC("RemovePin", PhotonTargets.All);
			}
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(isPinOnPumpking);
		}
		else
		{
			// Network player, receive data
			isPinOnPumpking = (bool) stream.ReceiveNext();
		}
	}
}
