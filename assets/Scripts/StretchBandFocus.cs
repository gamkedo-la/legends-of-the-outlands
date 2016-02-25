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
	float maxDist = 1.5f;
	Vector3 homePos;

	bool beenStretched = false;
	public float stretchScaleBasis;
	bool isStretching = false;

	public void playerLifted() {
		Debug.Log("playerLifted - Photon null? " + (photonView == null));
		if(photonView == null) {
			playerLift();
		} else {
			photonView.RPC("playerLift", PhotonTargets.All);
		}
	}

	public void playerDropped() {
		if(photonView == null) {
			playerDrop();
		} else {
			photonView.RPC("playerDrop", PhotonTargets.All);
		}
	}

	[PunRPC]
	void playerLift(){
		Debug.Log("stretching band");
		isStretching = true;
		beenStretched = true;
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
		homePos = transform.position;
		bandStretched.SetActive( isStretching );
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
		bandRight.Rotate(Vector3.up, 180.0f, Space.Self);

		stretchVec = Vector3.one * 0.01f;
		stretchVec.z *= (anchorPtRight.position - stretchFocus).magnitude / stretchScaleBasis;
		bandRight.transform.localScale = stretchVec;

		if(Vector3.Distance(transform.position, homePos) > maxDist) {
			isStretching = false;
		}

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
				Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Ignore Raycast"), false);

				// remove launched spool
				GameObject littleTape = GameObject.Find("TubeRenderer_Tape Measure Start-FromSpool");
				if(littleTape) {
					littleTape.SetActive(false);
				}
				gameObject.SetActive(false);
			} else if(beenStretched) {
				transform.position = stretchFocus;
			}
		}
	}
}
