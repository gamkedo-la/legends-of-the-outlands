using UnityEngine;
using System.Collections;

public class TriggerPlayAnimOnce : Photon.MonoBehaviour {
	public GameObject playAnimOn;
	public Collider colliderToVanish;

	bool playedYet = false;

	[PunRPC]
	public void FireAnim() {
		if(playedYet == false) {
			playedYet = true;
			Animator playWhichAnim = playAnimOn.GetComponent<Animator>();
			playWhichAnim.enabled = true;
			colliderToVanish.enabled = false;
		}
	}

	void OnTriggerEnter (Collider whoEnters) {
		if(whoEnters.tag == "Player") {
			if(photonView == null) {
				FireAnim();
			} else {
				photonView.RPC("FireAnim", PhotonTargets.All);
			}
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(playedYet);
		}
		else
		{
			// Network player, receive data
			bool wentYet = (playedYet);
			playedYet = (bool) stream.ReceiveNext();
			bool justWent = (playedYet);
			if(wentYet == false && justWent) {
				FireAnim();
			}
		}
	}

}
