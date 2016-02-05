using UnityEngine;
using System.Collections;

public class ClimbingController : MonoBehaviour {

    private Transform player;
    private PlayerMovement movementScript;
    [SerializeField] private bool climbing = false; //Duplicate in PlayerMovement
    int debugMessageCounter = 0;

	// Use this for initialization
	void Start () {
        player = transform.root;
        movementScript = transform.root.GetComponent<PlayerMovement>();
	}
	
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.name == "ClimbableTop" && climbing){
            movementScript.stopClimbing();
            player.position += player.up * 0.3f;
            player.position += player.forward * 0.5f;
            climbing = false;
        }
    }

    void OnTriggerStay(Collider collider){
        //If player hits bottom of climbing zone, don't go through floor
        if (climbing && collider.gameObject.name == "ClimbableBottom" && Input.GetAxis("Vertical") < 0)
        {
            movementScript.stopClimbing();
            climbing = false;
            //If player hits top of climbing zone, push player forward & up out of zone
        }
        //If player holds lmb in climbing zone, start climbing
        else if (!climbing && collider.gameObject.tag == "Climbable" && Input.GetMouseButtonDown(0)){
            movementScript.startClimbing();
            climbing = true;
        }
        else if(climbing && collider.gameObject.tag == "Climbable" && Input.GetKeyDown(KeyCode.Space)){
            movementScript.stopClimbing();
            climbing = false;
        }
    }

    void OnTriggerExit(Collider collider){
        //Stop climbing if player exits climing zone
        if (climbing && collider.gameObject.tag == "Climbable"){
            movementScript.stopClimbing();
            climbing = false;
        }
    }

    void Update(){
        //Stop climbing on mouse release
        if(climbing && Input.GetMouseButtonUp(0)){
            movementScript.stopClimbing();
            climbing = false;
            Debug.Log(debugMessageCounter++ + ": 0:1");
        }
    }
}
