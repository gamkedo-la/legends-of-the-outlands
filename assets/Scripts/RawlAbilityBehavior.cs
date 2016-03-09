using UnityEngine;
using System.Collections;

public class RawlAbilityBehavior : MonoBehaviour {

	private Material startMat;
	private Renderer rend;
	private Material highlightMat;
	private bool powerShowing = false;

	void Start () {
		rend = GetComponent<Renderer> ();
		startMat = rend.material;
		highlightMat = Resources.Load ("HighlightMat") as Material;
	}

	public static bool isLocalPlayerRawl (){
		GameObject Rawl = (GameObject) GameObject.Find ("Rawl(Clone)");
		if (Rawl == null) {
			return false;
		}

		PlayerMovement PMScript = Rawl.GetComponent<PlayerMovement> ();
		return PMScript.enabled;
	}
		

	void Update () {
		if(Input.GetKeyDown(KeyCode.Q) && isLocalPlayerRawl())
		{
			powerShowing = !powerShowing;
			if (powerShowing) {
				rend.material = highlightMat;
			} else {
				rend.material = startMat;
			}
		}
	}
}