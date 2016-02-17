using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingToastWaypoints : MonoBehaviour {
	public List<Vector3> waypointFlown;

	float RAT_HIGHER_THAN_BREAD_CENTER = 0.5f;

	List<float> timeStamps;
	Vector3 lastPos;
	float MIN_DIST = 0.2f;
	bool flownStarted = false;
	int wpNum = 2; // skip first two as bogus init data
	PlayerMovement flyingNow = null;

	// Use this for initialization
	void Start () {
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
			if(Time.time >= timeStamps[wpNum]) {
				wpNum++;
				if(wpNum >= waypointFlown.Count) {
					flyingNow.GetComponent<Collider>().enabled = true;
					flyingNow.enabled = true;
					flyingNow = null;
				}
			}
		}
	}

	public bool CarryMe(PlayerMovement flyingRat) {
		if(flownStarted == false && waypointFlown.Count > 3) {
			for(int i=timeStamps.Count-1;i>=2;i--) { // deliberately skipping first few from test plunge at start
				timeStamps[i] = timeStamps[i] - timeStamps[2] + Time.time; 
			}
			flyingNow = flyingRat;
			flyingNow.GetComponent<Collider>().enabled = false;
			flownStarted = true;
			return true;
		}
		return false;
	}
}
