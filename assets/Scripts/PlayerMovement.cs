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
    public bool isClimbing = false;
    public float maxGrabDistance = 1.5f;
    
    bool hasJumped = false;
    Rigidbody rb;
    public Transform carrying = null;
    float lastClickTime;
    float carryingHeightAdjustment;
    float carryDistance;
    float minCarryingDistance;

    void Start() {
        BoxCollider collider = GetComponent<BoxCollider>();
        minCarryingDistance = collider.bounds.extents.z + collider.center.z;
        rb = GetComponentInChildren<Rigidbody> ();
	}

	void Movement(){
        if (!isClimbing)
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
        else {
            transform.position += transform.up * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
            transform.position += transform.right * (Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"));
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

		if (Input.GetKeyDown (KeyCode.Space) && grounded == true) {
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
        isClimbing = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void stopClimbing(){
        isClimbing = false;
        GetComponent<Rigidbody>().useGravity = true;
    }

    private void handleCarrying(){
        //Pickup object
        if (Input.GetMouseButtonDown(0) && carrying == null){
            RaycastHit hit;

            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, maxGrabDistance) && hit.transform.tag == "Carryable"){
                carrying = hit.transform;
                carrying.GetComponent<Rigidbody>().isKinematic = true; //Carried object no longer affected by inertia or gravity

                //Determine how far the object must be from the player not to clip the floor or push the player backwards
                Vector3 carryingExtents = carrying.GetComponent<Collider>().bounds.extents;
                carryingHeightAdjustment = carryingExtents.y;
                carryDistance = Mathf.Max(Mathf.Sqrt(carryingExtents.x * carryingExtents.x + carryingExtents.y * carryingExtents.y) + minCarryingDistance,
                    Vector3.Distance(transform.position, new Vector3(carrying.position.x, transform.position.y, carrying.position.z)));
            }
        }
        //Drop object
        else if (Input.GetMouseButtonDown(0)){
            carrying.GetComponent<Rigidbody>().isKinematic = false; //Turn inertia and gravity back on
            carrying = null;
        }
        //If an object is being carried, update the position of the object
        if (carrying != null){
            carrying.position = transform.position + transform.forward * carryDistance + transform.up * carryingHeightAdjustment;
        }
    }
}
