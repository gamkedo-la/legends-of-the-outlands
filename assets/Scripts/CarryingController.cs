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

    void Start(){
        BoxCollider collider = GetComponent<BoxCollider>();
        minCarryingDistance = collider.bounds.extents.z + collider.center.z;
    }

    // Update is called once per frame
    void LateUpdate(){
        //Pickup object
        if (Input.GetMouseButtonDown(0) && carrying == null){
            RaycastHit hit;
			LayerMask lmask = ~LayerMask.GetMask("Ignore Raycast");

			if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, maxGrabDistance, lmask) && hit.transform.tag == "Carryable"){
                pickupObject(hit);
            }
            else if (hit.transform != null && hit.transform.tag == "Slideable"){
                slideObject(hit);
            }
        }
        else if (Input.GetMouseButtonDown(0)){
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
        hit.collider.SendMessage("playerLifted", SendMessageOptions.DontRequireReceiver);
        hit.transform.parent = carrying.transform;
        moveableOffset = carryingOffset;
    }

    //Marks an object as sliding, does not actually move the object
    void slideObject(RaycastHit hit){
        //Determine how far the object must be from the player not to clip the floor or push the player backwards
        carrying = hit.transform;
        carrying.GetComponent<Rigidbody>().isKinematic = true; //Carried object no longer affected by inertia or gravity
        Vector3 slidingExtents = carrying.GetComponent<Collider>().bounds.extents;
        moveableOffset = new Vector3(0, slidingExtents.y,
            Mathf.Min(Mathf.Sqrt(slidingExtents.x * slidingExtents.x + slidingExtents.y * slidingExtents.y) + minCarryingDistance,
                Vector3.Distance(transform.position, new Vector3(carrying.position.x, transform.position.y, carrying.position.z))));
        sliding = true;
    }

    //Drop carried object
    void releaseObject(){
        carrying.GetChild(0).SendMessage("playerDropped", SendMessageOptions.DontRequireReceiver);
        if (!sliding){
            //carrying has only one child: the object being carried
            carrying.GetChild(0).GetComponent<Collider>().enabled = true;
            carrying.GetChild(0).GetComponent<Rigidbody>().isKinematic = false; //Turn inertia and gravity back on
            carrying.GetChild(0).parent = carrying.parent;
        }
        else{
            carrying.GetComponent<Rigidbody>().isKinematic = false;
        }
        Destroy(carrying.gameObject); // accumulates new GO's without this
        carrying = null;
        sliding = false;
    }
}
