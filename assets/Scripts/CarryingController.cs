using UnityEngine;
using System.Collections;

public class CarryingController : Photon.MonoBehaviour{
    public float maxGrabDistance = 1.5f;
    public Vector3 carryingOffset = new Vector3(0, 0, 1.0f);

	int heldByPlayerID = -1;

    public bool sliding = false;
    Transform carrying = null;
    float minCarryingDistance; //TODO check to see if this is needed
    Vector3 moveableOffset;
    Transform storeOldParent;
	public bool wasObjectKinematic;
    EmAbilityBehavior emAbility;
	Rigidbody rb;
	LayerMask lmask;

    void Start(){
		lmask = ~LayerMask.GetMask("Ignore Raycast","DialogTrigger");

        BoxCollider collider = GetComponent<BoxCollider>();
		minCarryingDistance = collider.bounds.extents.z + collider.center.z + collider.bounds.extents.x;
        if(name == "Em"){
            emAbility = GetComponent<EmAbilityBehavior>();
        }
    }

    // Update is called once per frame
    void LateUpdate(){
        //Pickup object
		if (Input.GetButtonDown("Fire1") && carrying == null){
            RaycastHit hit;

            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, maxGrabDistance, lmask) && hit.transform.tag == "Carryable"){
				if(photonView == null) {
					pickupObject(PhotonNetwork.player.ID);
				} else {
					photonView.RPC("pickupObject", PhotonTargets.All, PhotonNetwork.player.ID);
				}
				// pickupObject(hit, PhotonNetwork.player.ID);
            }
            else if (hit.transform != null && hit.transform.tag == "Slideable"){
				if(photonView == null) {
					slideObject(PhotonNetwork.player.ID);
				} else {
					photonView.RPC("slideObject", PhotonTargets.All, PhotonNetwork.player.ID);
				}
				// slideObject(hit, PhotonNetwork.player.ID);
            }
        }
        else if (Input.GetButtonDown("Fire1")){
			if(photonView == null) {
				releaseObject();
			} else {
				photonView.RPC("releaseObject", PhotonTargets.All);
			}
            // releaseObject();
        }

        //If an object is being carried/slid, update the position of the object
		if (carrying != null /* && heldByPlayerID == PhotonNetwork.player.ID */){
			if(photonView == null) {
				moveObject();
			} else {
				photonView.RPC("moveObject", PhotonTargets.All);
			}
            // moveObject();
        }
    }

	[PunRPC]
    private void moveObject(){
		Vector3 goalPt = transform.position
			+ transform.forward * moveableOffset.z
			+ transform.up * moveableOffset.y
			+ transform.right * moveableOffset.x;;

		if(sliding) {
			goalPt.y = carrying.position.y;
		}
		if(carrying != null) {
			carrying.position = goalPt;
		} else {
			releaseObject();
			return;
		}

		if (!sliding) { carrying.rotation = transform.rotation; } //Only rotate if carrying, not sliding
    }

	[PunRPC]
	void pickupObject(int grabbedByPlayerID){
		RaycastHit hit;
		Physics.Raycast(new Ray(transform.position, transform.forward), out hit, maxGrabDistance, lmask);

		heldByPlayerID = grabbedByPlayerID;

        Transform carryingPoint = hit.transform.Find("CarryingPoint");
		GameObject carryGO = PhotonNetwork.Instantiate("CarryTransform", carryingPoint.position, carryingPoint.rotation, 0);
		carrying = /*new GameObject()*/ carryGO.transform;

        hit.collider.enabled = false;
		rb = hit.collider.GetComponentInChildren<Rigidbody>();
		wasObjectKinematic = rb.isKinematic;
		rb.isKinematic = true; //Carried object no longer affected by inertia or gravity
        carrying.parent = hit.transform.parent;

		PhotonView pvScript = hit.transform.GetComponent<PhotonView>();
		if(pvScript) {
			pvScript.RequestOwnership();
		}

        if (carryingPoint != null){
            carrying.position = carryingPoint.position;
            carrying.rotation = carryingPoint.rotation;
        }
        //Fallback code if there is no carrying point on a carryable obect
        else{
            Debug.Log("WARNING: OBJECT " + hit.transform.name + " LACKS CARRYING POINT\nOBJECT IS BEING CARRIED BY THE CENTER");
            carrying.position = hit.transform.position;
            carrying.rotation = hit.transform.rotation;
        }
        hit.collider.SendMessage("playerLifted", SendMessageOptions.DontRequireReceiver);
        hit.transform.parent = carrying.transform;
        moveableOffset = carryingOffset;
    }

    //Marks an object as sliding, does not actually move the object
	[PunRPC]
	void slideObject(int grabbedByPlayerID){
		RaycastHit hit;
		Physics.Raycast(new Ray(transform.position, transform.forward), out hit, maxGrabDistance, lmask);

		heldByPlayerID = grabbedByPlayerID;

        //Determine how far the object must be from the player not to clip the floor or push the player backwards
        carrying = hit.transform;

        setEmAbility(false);

		rb = hit.collider.GetComponent<Rigidbody>();
		wasObjectKinematic = rb.isKinematic;
		rb.isKinematic = true; //Carried object no longer affected by inertia or gravity

        Vector3 slidingExtents = carrying.GetComponent<Collider>().bounds.extents;
        moveableOffset = new Vector3(0, slidingExtents.y,
            1.2f+ Mathf.Min(Mathf.Sqrt(slidingExtents.x * slidingExtents.x + slidingExtents.y * slidingExtents.y) + minCarryingDistance,
                Vector3.Distance(transform.position, new Vector3(carrying.position.x, transform.position.y, carrying.position.z))));
        sliding = true;
    }

    //Drop carried object
	[PunRPC]
    void releaseObject(){
		/* if(heldByPlayerID != PhotonNetwork.player.ID) {
			return;
		}*/
		if(carrying == null) {
			return;
		}

		heldByPlayerID = -1;

		if(carrying.childCount > 0) {
			carrying.GetChild(0).SendMessage("playerDropped", SendMessageOptions.DontRequireReceiver);
		}

        if (!sliding){
            //Check to see if carried object has a special script attatched for dropping it
            CarryableObject carryableObject = carrying.GetChild(0).GetComponent<CarryableObject>();

            //if it does, then execute it
            if (carryableObject != null){
                RaycastHit hit;
                Physics.Raycast(new Ray(transform.position, transform.forward), out hit);
                carryableObject.dropObject(hit);
            }
            //otherwise just drop it
            else{
                carrying.GetChild(0).GetComponent<Collider>().enabled = true;
                carrying.GetChild(0).GetComponent<Rigidbody>().isKinematic = wasObjectKinematic; //Turn inertia and gravity back on
                carrying.GetChild(0).transform.parent = carrying.GetChild(0).transform.parent.parent;
            }
            Destroy(carrying.gameObject); // accumulates new GO's without this
        }
        else{
            carrying.GetComponent<Rigidbody>().isKinematic = wasObjectKinematic;
        }
        carrying = null;
        sliding = false;

        setEmAbility(true);
    }

    void setEmAbility(bool abilityState)
    {
        if (emAbility != null)
        {
            emAbility.changeStatus(abilityState);
        }
    }

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			if(carrying != null) {
				stream.SendNext(carrying.position);
				stream.SendNext(carrying.rotation);
			}
		}
		else
		{
			// Network player, receive data
			if(carrying != null) {
				carrying.transform.position = (Vector3)stream.ReceiveNext();
				carrying.transform.rotation = (Quaternion)stream.ReceiveNext();
			}
		}
	}
}
