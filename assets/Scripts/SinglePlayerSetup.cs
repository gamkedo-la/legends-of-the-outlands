using UnityEngine;
using System.Collections;

public class SinglePlayerSetup : MonoBehaviour {

	public GameObject whichRat;

	public void SinglePlayer(Transform spawnPoint){
		GameObject monster = Instantiate (whichRat, spawnPoint.position, Quaternion.identity) as GameObject; 
		PlayerMovement controller = monster.GetComponent<PlayerMovement> ();
		controller.enabled = true;
		NetworkCharacter networkOptions = monster.GetComponent<NetworkCharacter> ();
		networkOptions.enabled = false;
	}
}
