using UnityEngine;
using System.Collections;

public class AnimateWhenRatHere : Photon.MonoBehaviour {
	public Animator someAnim;

	void Start (){
		PhotonView photonView = PhotonView.Get(this);
	}

	void OnTriggerEnter (Collider other) {
		if(other.GetComponent<Collider>().name.StartsWith("Rawl")) {
			photonView.RPC("WeighDown", PhotonTargets.All);
		}
		
	}

	void OnTriggerExit (Collider other) {
		if(other.GetComponent<Collider>().name.StartsWith("Rawl")) {
			photonView.RPC("WeighUp", PhotonTargets.All);
		}
	}

	[PunRPC]
	void WeighDown () {
		someAnim.SetBool("WeighedDown", true);
	}

	[PunRPC]
	void WeighUp () {
		someAnim.SetBool("WeighedDown", false);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext( someAnim.GetBool("WeighedDown") );
		}
		else
		{
			someAnim.SetBool("WeighedDown",  (bool) stream.ReceiveNext() );
		}
	}
}
