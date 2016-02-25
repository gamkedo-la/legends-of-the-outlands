using UnityEngine;
using System.Collections;

public class CarryingController : MonoBehaviour{
    public float maxGrabDistance = 1.5f;
    public Vector3 carryingOffset = new Vector3(0, 0, 1.0f);

    public bool sliding = false;
    Transform carrying = null;
    float minCarryingDistance; //TODO check to see if this is needed
    Vector3 moveableOffset;
    Transform storeOldParent;
	public bool wasObjectKinematic;
    EmAbilityBehavior emAbility;

    void Start(){
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
			LayerMask lmask = ~LayerMask.GetMask("Ignore Raycast");

            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, maxGrabDistance, lmask) && hit.transform.tag == "Carryable"){
                pickupObject(hit);
            }
            else if (hit.transform != null && hit.transform.tag == "Slideable"){
                slideObject(hit);
            }
        }
        else if (Input.GetButtonDown("Fire1")){
            releaseObject();
        }

        //If an object is being carried/slid, update the position of the object
        if (carrying != null){
            moveObject();
        }
    }

    private void moveObject(){
        carrying.position = transform.position
            + transform.forward * moveableOffset.z
            + transform.up * moveableOffset.y
            + transform.right * moveableOffset.x;
        if (!sliding) { carrying.rotation = transform.rotation; } //Only rotate if carrying, not sliding
    }

    void pickupObject(RaycastHit hit){
        Transform carryingPoint = hit.transform.Find("CarryingPoint");
        carrying = new GameObject().transform;

        hit.collider.enabled = false;
		wasObjectKinematic = hit.collider.GetComponent<Rigidbody>().isKinematic;
        hit.collider.GetComponent<Rigidbody>().isKinematic = true; //Carried object no longer affected by inertia or gravity
        carrying.parent = hit.transform.parent;

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
//        hit.collider.SendMessage("playerLifted", SendMessageOptions.DontRequireReceiver);
        hit.transform.parent = carrying.transform;
        moveableOffset = carryingOffset;
    }

    //Marks an object as sliding, does not actually move the object
    void slideObject(RaycastHit hit){
        //Determine how far the object must be from the player not to clip the floor or push the player backwards
        carrying = hit.transform;

        if(emAbility != null){
            emAbility.changeStatus(false);
        }
		wasObjectKinematic = hit.collider.GetComponent<Rigidbody>().isKinematic;
        carrying.GetComponent<Rigidbody>().isKinematic = true; //Carried object no longer affected by inertia or gravity

        Vector3 slidingExtents = carrying.GetComponent<Collider>().bounds.extents;
        moveableOffset = new Vector3(0, slidingExtents.y - 0.1f,
            Mathf.Min(Mathf.Sqrt(slidingExtents.x * slidingExtents.x + slidingExtents.y * slidingExtents.y) + minCarryingDistance,
                Vector3.Distance(transform.position, new Vector3(carrying.position.x, transform.position.y, carrying.position.z))));
        sliding = true;
    }

    //Drop carried object
    void releaseObject(){
        //carrying.GetChild(0).SendMessage("playerDropped", SendMessageOptions.DontRequireReceiver);
        RaycastHit hit;

        if (!sliding){
            //Check to see if carried object has a special script attatched for dropping it
            CarryableObject carryableObject = carrying.GetChild(0).GetComponent<CarryableObject>();

            //if it does, then execute it
            if (carryableObject != null){
                if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit)){
                    carryableObject.dropObject(hit);
                }
                else{
                    carryableObject.dropObject();
                }
            }
            //otherwise just drop it
            else{
                carrying.GetChild(0).GetComponent<Collider>().enabled = true;
                carrying.GetChild(0).GetComponent<Rigidbody>().isKinematic = wasObjectKinematic; //Turn inertia and gravity back on
                carrying.GetChild(0).transform.parent = carrying.GetChild(0).transform.parent.parent;
            }
            Destroy(carrying.gameObject); // accumulates new GO's without this
        }
        else
        {
            carrying.GetComponent<Rigidbody>().isKinematic = wasObjectKinematic;
        }
        carrying = null;
        sliding = false;

        if(emAbility != null){
            emAbility.changeStatus(true);
        }
    }
}
