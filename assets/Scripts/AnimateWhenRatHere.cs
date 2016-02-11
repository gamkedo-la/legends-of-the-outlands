using UnityEngine;
using System.Collections;

public class AnimateWhenRatHere : Photon.MonoBehaviour {
	public Animator someAnim;

	void Start (){
		PhotonView photonView = PhotonView.Get(this);
	}

	void OnTriggerEnter (Collider other) {
		photonView.RPC("WeighDown", PhotonTargets.All);
		
	}

	void OnTriggerExit (Collider other) {
		photonView.RPC("WeighUp", PhotonTargets.All);
	}

	[PunRPC]
	void WeighDown () {
		someAnim.SetBool("WeighedDown", true);
	}

	[PunRPC]
	void WeighUp () {
		someAnim.SetBool("WeighedDown", false);
	}
}
