using UnityEngine;
using System.Collections;

public class ClimbingController : MonoBehaviour {

    private PlayerMovement movementScript = new PlayerMovement();
    public float maxStartClimbingAngle = 60;
    float climbingDistanceOffset = -0.1f;
    float topVerticalAdjustment = 0.3f, topHorizontalAdjustment = 1.0f;
    bool lmbDown = false;
    bool movingForward = false;
    bool inClimbableBottom = false, inClimbable = false, inClimbableTop = false;

	// Use this for initialization
	void Start () {
        movementScript = GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider collider){
        //Cache which part of the climbing collider the player is in
        if (collider.gameObject.name == "ClimbableBottom") { inClimbableBottom = true; }
        if (collider.gameObject.name == "ClimbableTop") { inClimbableTop = true; }
        if (collider.gameObject.tag == "Climbable") { inClimbable = true; }
    }

    void OnTriggerStay(Collider collider){
        //If player holds lmb in climbing zone, start climbing
        if (!movementScript.climbing && lmbDown && movingForward && collider.gameObject.tag == "Climbable" && !inClimbableTop){
            lmbDown = false;
            Vector3 colliderForward = collider.GetComponent<Transform>().forward;

            //If the player is facing the same way as the climbing collider
            if (Vector3.Angle(transform.forward, colliderForward) < maxStartClimbingAngle){
                
                //Look in the direction the climbing collider is facing
                transform.rotation = Quaternion.LookRotation(colliderForward);
                
                //Adjust distance to the climbing collider
                transform.position += (Vector3.Dot((transform.position - collider.transform.position),(-collider.transform.forward)) + climbingDistanceOffset) * transform.forward;

                //Change to climbing movement
                movementScript.startClimbing();
            }
        }
    }

    void OnTriggerExit(Collider collider){
        //Cache which part of the climbing collider the player just left
        if (collider.gameObject.name == "ClimbableBottom") { inClimbableBottom = false; }
        if (collider.gameObject.name == "ClimbableTop") { inClimbableTop = false; }

        //Stop climbing if player exits climing zone
        if (movementScript.climbing && collider.gameObject.tag == "Climbable"){
            movementScript.stopClimbing();
            inClimbable = false;
        }
    }

    void Update(){
        //Cache inputs
		if (Input.GetButtonDown("Fire1")){
            lmbDown = true;
        }
		else if(Input.GetButtonUp("Fire1")){
            lmbDown = false;
        }

        if(Input.GetAxis("Vertical") > 0){
            movingForward = true;
        }
        else{
            movingForward = false;
        }

        if (movementScript.climbing) {
            //If player hits bottom of climbing zone, don't go through floor
            if (inClimbableBottom && Input.GetAxis("Vertical") < 0) {
                movementScript.stopClimbing();
            }
            //Stop climbing on mouse release
			else if (Input.GetButtonUp("Fire1")){
                movementScript.stopClimbing();
            }
            //Stop climbing if the player jumps
            else if (inClimbable && Input.GetKeyDown(KeyCode.Space)){
                movementScript.stopClimbing();
            }
            //If player hits top of climbing zone, push player forward & up out of zone
            if (inClimbableTop){
                movementScript.stopClimbing();
                transform.position += transform.up * topVerticalAdjustment + transform.forward * topHorizontalAdjustment;
            }
        }
    }
}
