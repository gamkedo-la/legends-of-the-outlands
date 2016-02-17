using UnityEngine;
using System.Collections;

public class StretchBandFocus : Photon.MonoBehaviour {

	public GameObject bandWhole;
	public GameObject bandStretched;

	public Transform anchorPtLeft;
	public Transform anchorPtRight;

	public Transform bandLeft;
	public Transform bandRight;

	public GameObject showBridge;

	Vector3 stretchFocus;

	float stretchScaleBasis;
	bool isStretching = false;

	public void playerLifted() {
		photonView.RPC("playerLift", PhotonTargets.All);
	}

	public void playerDropped() {
		photonView.RPC("playerDrop", PhotonTargets.All);
	}

	[PunRPC]
	void playerLift(){
		Debug.Log("stretching band");
		isStretching = true;
		bandWhole.SetActive(isStretching == false);
		bandStretched.SetActive( isStretching );
	}

	[PunRPC]
	void playerDrop(){
		Debug.Log("band released");
		isStretching = false;
	}

	void OnEnable() {
		showBridge.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		PhotonView photonView = PhotonView.Get(this);
		stretchFocus = transform.position;
		isStretching = false;
		bandWhole.SetActive(isStretching == false);
		bandStretched.SetActive( isStretching );
		stretchScaleBasis = 7.9f - 5.421f; // length at default scale, found by moving between those coords on axis
	}
	
	// Update is called once per frame
	void Update () {
		if(isStretching) {
			stretchFocus = transform.position;
		}

		bandLeft.position = (stretchFocus + anchorPtLeft.position) * 0.5f;
		bandLeft.LookAt(stretchFocus);

		Vector3 stretchVec = Vector3.one * 0.01f;
		stretchVec.z *= (anchorPtLeft.position - stretchFocus).magnitude / stretchScaleBasis;
		bandLeft.transform.localScale = stretchVec;

		bandRight.position = (stretchFocus + anchorPtRight.position) * 0.5f;
		bandRight.LookAt(stretchFocus);
		bandRight.Rotate(transform.up, 180.0f, Space.Self);

		stretchVec = Vector3.one * 0.01f;
		stretchVec.z *= (anchorPtRight.position - stretchFocus).magnitude / stretchScaleBasis;
		bandRight.transform.localScale = stretchVec;
	}

	void FixedUpdate() {
		if(isStretching == false) {
			Vector3 centerTgt = (anchorPtLeft.position+anchorPtRight.position) * 0.5f;
			float elasticBias = 0.7f;
			stretchFocus = stretchFocus * elasticBias + centerTgt * (1.0f - elasticBias);
			if(bandWhole.activeSelf == false && Vector3.Distance(centerTgt, stretchFocus) < 0.1f) {
				bandWhole.SetActive(true);
				bandStretched.SetActive(false);
				showBridge.SetActive(true);
			}
		}
	}
}
