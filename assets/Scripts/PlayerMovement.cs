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
    public Transform carrying = null;
    public float grabDistance = 1.5f;
    public float minCarryDistance = 1f;

    bool hasJumped = false;
	float lastClickTime;
    float carryingHeightAdjustment;
	Rigidbody rb;
    float carryDistance;

	void Start() {
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
        }else{
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
			} else {
				SoundManager.instance.PlayNamedClipOn (squeak, transform.position, gameObject.name, "squeak", 1.0f, transform);	
			}
			lastClickTime = Time.time;
		}
		Movement ();

        if (Input.GetMouseButtonDown(0) && carrying == null){
            grab();
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, grabDistance))
            {
                if (hit.transform.tag == "Carryable")
                {
                    carrying = hit.transform;
                    carrying.GetComponent<Rigidbody>().isKinematic = true;
                    carryingHeightAdjustment = carrying.GetComponent<Renderer>().bounds.extents.y;
                    carryDistance = hit.distance < minCarryDistance ? minCarryDistance : hit.distance;
                    Debug.Log("Carry dist: " + carryDistance);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)){
            release();
        }

        if(carrying != null){
            carrying.position = transform.position + transform.forward * carryDistance + transform.up * carryingHeightAdjustment;
        }
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

    void grab(){
    }

    void release(){
        carrying.GetComponent<Rigidbody>().isKinematic = false;
        carrying = null;
    }
}
