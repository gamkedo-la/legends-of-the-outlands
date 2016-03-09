using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingToastWaypoints : MonoBehaviour {
	public List<Vector3> waypointFlown;

	public TriggerPlayAnimOnce activateLatchIfSP;

	float RAT_HIGHER_THAN_BREAD_CENTER = 0.5f;

	List<float> timeStamps;
	Vector3 lastPos;
	float MIN_DIST = 0.2f;
	bool flownStarted = false;

	int SKIP_FIRST_X_WAYPOINTS = 7; // skip first handful as bogus in-toaster data

	int wpNum;
	PlayerMovement flyingNow = null;
	float launchTime = 0.0f;

	// Use this for initialization
	void Start () {
		wpNum = SKIP_FIRST_X_WAYPOINTS;
		lastPos = transform.position;
		waypointFlown = new List<Vector3>();
		timeStamps = new List<float>();
	}

	// Update is called once per frame
	void Update () {
		if( flownStarted == false && Vector3.Distance( lastPos, transform.position ) > MIN_DIST) {
			lastPos = transform.position;
			waypointFlown.Add(lastPos);
			timeStamps.Add(Time.time);
		}
		if(flyingNow) {
			flyingNow.transform.position = waypointFlown[wpNum] + Vector3.up * RAT_HIGHER_THAN_BREAD_CENTER;
			if(Time.time >= timeStamps[wpNum] - timeStamps[SKIP_FIRST_X_WAYPOINTS] + launchTime) {
				wpNum++;
				if(wpNum >= waypointFlown.Count) {
					// flyingNow.GetComponent<Collider>().enabled = true;
					// flyingNow.enabled = true;
					Debug.Log("Launch complete");
					wpNum = SKIP_FIRST_X_WAYPOINTS; // reload in case they want to do it again
					flyingNow = null;
					flownStarted = false;
					if(RandomMatchmaker.instance && RandomMatchmaker.instance.singlePlayer && activateLatchIfSP) {
						Debug.Log("Calling FireAnim");
						activateLatchIfSP.FireAnim();
					}
				}
			}
		}
	}

	public bool CarryMe(PlayerMovement flyingRat) {
		if(flownStarted == false && waypointFlown.Count > SKIP_FIRST_X_WAYPOINTS) {
			launchTime = Time.time;
			flyingNow = flyingRat;
			// flyingNow.GetComponent<Collider>().enabled = false;
			flownStarted = true;
			return true;
		}
		return false;
	}
}
