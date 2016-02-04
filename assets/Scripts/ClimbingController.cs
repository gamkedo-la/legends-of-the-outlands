using UnityEngine;
using System.Collections;

public class ClimbingController : MonoBehaviour {

    private Transform player;
    private PlayerMovement movementScript;
    private bool climbing = false; //Duplicate in PlayerMovement

	// Use this for initialization
	void Start () {
        player = transform.root;
        movementScript = transform.root.GetComponent<PlayerMovement>();
	}
	
    void OnTriggerEnter(Collider collider){
        //If player hits bottom of climbing zone, don't go through floor
        if(collider.gameObject.name == "ClimbableBottom" && Input.GetKeyDown(KeyCode.S))
        {
            movementScript.stopClimbing();
            climbing = false;
        //If player hits top of climbing zone, push player forward & up out of zone
        }else if(collider.gameObject.name == "ClimbableTop" && climbing){
            movementScript.stopClimbing();
            player.position += player.up * 0.1f;
            player.position += player.forward * 0.5f;
            climbing = false;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        //If player holds lmb in climbing zone, start climbing
        if (!climbing && collider.gameObject.tag == "Climbable" && Input.GetMouseButtonDown(0))
        {
            movementScript.startClimbing();
            climbing = true;
        }
    }

    void OnTriggerExit(Collider collider){
        //Stop climbing if player exits climing zone
        if (climbing && collider.gameObject.tag == "Climbable"){
            movementScript.stopClimbing();
            climbing = false;
        }
    }

    void FixedUpdate(){
        //Stop climbing on mouse release
        if(climbing && Input.GetMouseButtonUp(0)){
            movementScript.stopClimbing();
            climbing = false;
        }
    }
}
