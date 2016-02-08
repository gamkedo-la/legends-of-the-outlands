using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float rotateSpeed = 50.0f;
	public float catchTime = 0.25f;
	public AudioClip squeak;
	public AudioClip chatter;
	public float jumpPower = 20.0f;
	public bool grounded = true;
    public bool climbing = false;
    public float maxGrabDistance = 1.5f;
    public Vector3 carryingOffset = new Vector3(0, 0, 1.0f);
    
    bool hasJumped = false;
    bool sliding = false;
    Rigidbody rb;
    Transform carrying = null;
    float lastClickTime;
    float minCarryingDistance;
    Vector3 moveableOffset;
    Transform storeOldParent;

    void Start() {
        BoxCollider collider = GetComponent<BoxCollider>();
        minCarryingDistance = collider.bounds.extents.z + collider.center.z;
        rb = GetComponentInChildren<Rigidbody> ();
    }

	void Movement(){
        if (!climbing && !sliding)
        {
            transform.position += transform.forward * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
            transform.position += transform.right * (Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"));

            if (Input.GetAxis("Mouse X") < 0.0f)
            {
                transform.Rotate(0, -1.0f * Time.deltaTime * rotateSpeed, 0);
            }
            else if (Input.GetAxis("Mouse X") > 0.0f)
            {
                transform.Rotate(0, 1.0f * Time.deltaTime * rotateSpeed, 0);
            }
        }
        else if(climbing){
            transform.position += transform.up * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
            transform.position += transform.right * (Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"));
        }
        else{
            transform.position += transform.forward * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
        }
    }
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			transform.rotation = Quaternion.identity;
		}

		if (Input.GetKeyDown (KeyCode.Space) && grounded == true && !sliding) {
			hasJumped = true;
		}

		if (hasJumped) {
			rb.AddForce (transform.up * jumpPower);
			grounded = false;
			hasJumped = false;
		}
			
		if (Input.GetMouseButtonDown (1)) {
			if (Time.time - lastClickTime < catchTime) {
				SoundManager.instance.PlayNamedClipOn (chatter, transform.position, gameObject.name, "chatter", 1.0f, transform);	
			}
            else {
				SoundManager.instance.PlayNamedClipOn (squeak, transform.position, gameObject.name, "squeak", 1.0f, transform);	
			}
			lastClickTime = Time.time;
		}
		Movement ();

        handleCarrying();
	}

	void FixedUpdate(){
		// moved to fixedupdate so after force affects velocity
		if (grounded == false && Mathf.Abs(rb.velocity.y) < 0.01f) {
			grounded = true;
		}
	}

    public void startClimbing(){
        climbing = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void stopClimbing(){
        climbing = false;
        GetComponent<Rigidbody>().useGravity = true;
    }

    private void handleCarrying(){
        //Pickup object
        if (Input.GetMouseButtonDown(0) && carrying == null) {
            RaycastHit hit;

            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, maxGrabDistance) && hit.transform.tag == "Carryable")
            {
                pickupObject(hit);
            }
            else if (hit.transform != null && hit.transform.tag == "Slideable")
            {
                //-----------------------------RESUSE FOR PUSH/PULL-----------------------------------
                //Determine how far the object must be from the player not to clip the floor or push the player backwards
                carrying = hit.transform;
                carrying.GetComponent<Rigidbody>().isKinematic = true; //Carried object no longer affected by inertia or gravity
                Vector3 slidingExtents = carrying.GetComponent<Collider>().bounds.extents;
                moveableOffset = new Vector3(0, slidingExtents.y,
                    Mathf.Min(Mathf.Sqrt(slidingExtents.x * slidingExtents.x + slidingExtents.y * slidingExtents.y) + minCarryingDistance,
                        Vector3.Distance(transform.position, new Vector3(carrying.position.x, transform.position.y, carrying.position.z))));
                sliding = true;
            }
        }
        //Drop object
        else if (Input.GetMouseButtonDown(0))
        {
            releaseObject();
        }
        //If an object is being carried/slid, update the position of the object
        if (carrying != null){
            carrying.position = transform.position
                + transform.forward * moveableOffset.z
                + transform.up * moveableOffset.y
                + transform.right * moveableOffset.x;
            if (!sliding) { carrying.rotation = transform.rotation; } //Only rotate if carrying, not sliding
        }
    }

    void pickupObject(RaycastHit hit){
        Transform carryingPoint = hit.transform.Find("CarryingPoint");
        carrying = new GameObject().transform;

        hit.collider.enabled = false;
        hit.collider.GetComponent<Rigidbody>().isKinematic = true; //Carried object no longer affected by inertia or gravity
        carrying.parent = hit.transform.parent;

        if (carryingPoint != null)
        {
            carrying.position = carryingPoint.position;
            carrying.rotation = carryingPoint.rotation;
        }
        //Fallback code if there is no carrying point on a carryable obect
        else
        {
            Debug.Log("WARNING: OBJECT " + hit.transform.name + " LACKS CARRYING POINT\nOBJECT IS BEING CARRIED BY THE CENTER");
            carrying.position = hit.transform.position;
            carrying.rotation = hit.transform.rotation;
        }
        hit.transform.parent = carrying.transform;
        moveableOffset = carryingOffset;
    }

    //Drop carried object
    void releaseObject(){
        if (!sliding) {
            carrying.GetChild(0).GetComponent<Collider>().enabled = true;
            carrying.GetChild(0).GetComponent<Rigidbody>().isKinematic = false; //Turn inertia and gravity back on
            carrying.GetChild(0).parent = carrying.parent;
        }
        else{
            carrying.GetComponent<Rigidbody>().isKinematic = false;
        }
        carrying = null;
        sliding = false;
    }
}
