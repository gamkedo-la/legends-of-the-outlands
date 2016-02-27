using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour{

    public float movementSpeed = 5.0f;
    public float rotateSpeed = 50.0f;
    public float catchTime = 0.25f;
    public AudioClip squeak;
    public AudioClip chatter;
    public float jumpPower = 20.0f;
    public bool grounded = true;
    public bool climbing = false;
	public bool oldTransformMove = true;

	bool cursorShouldBeLocked = true;

    bool hasJumped = false;
    Rigidbody rb;
    float lastClickTime;
    CarryingController carryingController;
	PhotonView myPhotonView;
    EmAbilityBehavior emAbility;

    void Start(){
        rb = GetComponentInChildren<Rigidbody>();
        TP_Camera.UseExistingOrCreateNewMainCamera();
        carryingController = GetComponent<CarryingController>();
		myPhotonView = GetComponent<PhotonView> ();
        if(name == "Em")
        {
            emAbility = GetComponent<EmAbilityBehavior>();
        }
    }
	[PunRPC]
	void PlayChatter(){
		SoundManager.instance.PlayNamedClipOn(chatter, transform.position, gameObject.name, "chatter", 1.0f, transform);
	}

	[PunRPC]
	void PlaySqueak(){
		SoundManager.instance.PlayNamedClipOn(squeak, transform.position, gameObject.name, "squeak", 1.0f, transform);
	}

    void Movement(){
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			GameObject endGO = GameObject.Find("Debug Teleport 1");
			if(endGO) {
				transform.position = endGO.transform.position;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			GameObject endGO = GameObject.Find("Debug Teleport 2");
			if(endGO) {
				transform.position = endGO.transform.position;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			GameObject endGO = GameObject.Find("Debug Teleport 3");
			if(endGO) {
				GameObject sendGO = GameObject.Find("Em(Clone)");
				if(sendGO) {
					sendGO.transform.position = endGO.transform.position;
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			GameObject endGO = GameObject.Find("Debug Teleport 4");
			if(endGO) {
				transform.position = endGO.transform.position;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			GameObject endGO = GameObject.Find("Debug Teleport 5");
			if(endGO) {
				transform.position = endGO.transform.position;
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha6)) {
			GameObject endGO = GameObject.Find("Debug Teleport 6");
			if (endGO) {
				transform.position = endGO.transform.position;
			}
		}
        if (!climbing && !carryingController.sliding){
			Quaternion angleNow = transform.rotation;
			Quaternion angleGoal = Quaternion.LookRotation(Quaternion.AngleAxis(TP_Camera.Instance.mouseX, Vector3.up) * Vector3.forward);
            //if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.5f){
				transform.rotation = Quaternion.Slerp(angleNow, angleGoal, Time.deltaTime * 15.0f);
                //				transform.LookAt (transform.position + Quaternion.AngleAxis (TP_Camera.Instance.mouseX, Vector3.up) * Vector3.forward);
            //}
			// condition added so rat will turn before walking, helps for the game's balancing narrow areas
			float angDiff = Quaternion.Angle(angleGoal, angleNow);
			Vector3 moveEffect;

			if(oldTransformMove) {
				moveEffect = transform.forward * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
				if(angDiff < 15.0f || Input.GetAxis("Vertical") < 0.0f) {
					transform.position += moveEffect;
				} else {
					transform.position += moveEffect * 0.45f;
				}
				transform.position += transform.right * (Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"));
			} else {
				moveEffect = transform.forward * (movementSpeed * Input.GetAxis("Vertical")) + transform.right * (movementSpeed * Input.GetAxis("Horizontal"));
				float wasYVforGrav = rb.velocity.y;
				moveEffect.y = wasYVforGrav;
				rb.velocity = moveEffect;
			}

        }
        else if (climbing){
            transform.position += transform.up * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
            transform.position += transform.right * (Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"));
        }
        else{
			Vector3 moveEffect = transform.forward * (Time.deltaTime * movementSpeed * Input.GetAxis("Vertical"));
			if(carryingController.sliding) {
				moveEffect *= 10.7f;
			}
			rb.velocity = moveEffect;
        }
    }

    // Update is called once per frame
    void Update(){
		if (cursorShouldBeLocked)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

        if (Input.GetKeyDown(KeyCode.Escape)){
			cursorShouldBeLocked = false; // in case in editor
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded == true && !carryingController.sliding){
            hasJumped = true;
        }

        if (hasJumped){
            rb.AddForce(transform.up * jumpPower);
            grounded = false;
            hasJumped = false;
        }

        if (Input.GetMouseButtonDown(1)){
            if (Time.time - lastClickTime < catchTime){
				myPhotonView.RPC ("PlayChatter", PhotonTargets.All);
//				PlayChatter ();
                
            }
            else{
				myPhotonView.RPC ("PlaySqueak", PhotonTargets.All);
//				PlaySqueak ();
            }
            lastClickTime = Time.time;
        }
        Movement();
    }

    void FixedUpdate(){
        // moved to fixedupdate so after force affects velocity
        if (grounded == false && Mathf.Abs(rb.velocity.y) < 0.01f){
            grounded = true;
        }
    }

    public void startClimbing(){
        climbing = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameObject ratBody = GameObject.Find("Ratv2");
        ratBody.transform.position += transform.forward * 0.25f;
        ratBody.transform.Rotate(0.0f, 0.0f, +90.0f);
    }

    public void stopClimbing(){
        climbing = false;
        GetComponent<Rigidbody>().useGravity = true;
        GameObject ratBody = GameObject.Find("Ratv2");
        ratBody.transform.position -= transform.forward * 0.25f;
        ratBody.transform.Rotate(0.0f, 0.0f, -90.0f);
    }
}
