using UnityEngine;
using System.Collections;

public class ClimbingController : MonoBehaviour {

    private Transform player;
    private PlayerMovement movementScript;
    public float maxStartClimbingAngle = 60;
    float topVerticalAdjustment = 0.3f, topHorizontalAdjustment = 0.5f;
    bool lmbDown = false;
    bool inClimbableBottom = false, inClimbable = false, inClimbableTop = false;

	// Use this for initialization
	void Start () {
        player = transform.root;
        movementScript = transform.root.GetComponent<PlayerMovement>();
	}
	
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.name == "ClimbableBottom") { inClimbableBottom = true; }
        if (collider.gameObject.name == "ClimbableTop") { inClimbableTop = true; }
        if (collider.gameObject.tag == "Climbable") { inClimbable = true; }
    }

    void OnTriggerStay(Collider collider){
        //If player holds lmb in climbing zone, start climbing
        if (!movementScript.isClimbing && collider.gameObject.tag == "Climbable" && lmbDown){
            Vector3 colliderForward = collider.GetComponent<Transform>().forward;

            if (Vector3.Angle(player.forward, colliderForward) < maxStartClimbingAngle){
                player.rotation = Quaternion.LookRotation(colliderForward);
                movementScript.startClimbing();
            }
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject.name == "ClimbableBottom") { inClimbableBottom = false; }
        if (collider.gameObject.name == "ClimbableTop") { inClimbableTop = false; }

        //Stop climbing if player exits climing zone
        if (movementScript.isClimbing && collider.gameObject.tag == "Climbable"){
            movementScript.stopClimbing();
            inClimbable = false;
        }
    }

    void Update(){
        if (Input.GetMouseButtonDown(0)){
            lmbDown = true;
        }
        else if (Input.GetMouseButtonUp(0)){
            lmbDown = false;
        }

        if (movementScript.isClimbing) {
            //If player hits bottom of climbing zone, don't go through floor
            if (inClimbableBottom && Input.GetAxis("Vertical") < 0) {
                movementScript.stopClimbing();
            }
            //Stop climbing on mouse release
            else if (Input.GetMouseButtonUp(0)){
                movementScript.stopClimbing();
            }
            //Stop climbing if the player jumps
            else if (inClimbable && Input.GetKeyDown(KeyCode.Space)){
                movementScript.stopClimbing();
            }
            //If player hits top of climbing zone, push player forward & up out of zone
            if (inClimbableTop){
                movementScript.stopClimbing();
                player.position += player.up * topVerticalAdjustment + transform.forward * topHorizontalAdjustment;
            }
        }
    }
}
