using UnityEngine;
using System.Collections;

public class EmAbilityBehavior : MonoBehaviour {
    GameObject emAbilityObjects;
    Collider[] emAbilityZones = new Collider[1];
    Transform[] emAbilityTargets = new Transform[1];
    bool[] inZone;
    bool usingAbility = false;
    Camera emAbilityCam;
    Camera mainCam;
    bool isEnabled = true;

	// Use this for initialization
	void Start () {
		if(RandomMatchmaker.instance && RandomMatchmaker.instance.singlePlayer) {
			enabled = false;
			return;
		}

        GameObject abilityCamObject = new GameObject();
        abilityCamObject.AddComponent<Camera>();
        emAbilityCam = abilityCamObject.GetComponent<Camera>();
        emAbilityCam.enabled = false;
        mainCam = Camera.main;
        emAbilityObjects = GameObject.FindWithTag("EmAbilityObject");

        Transform emAbilityObjectsChild = emAbilityObjects.transform.GetChild(0);
        if(emAbilityObjectsChild.name == "EmAbilityZones"){
            // Debug.Log("init zones");
            findEmZones(emAbilityObjectsChild);
        }
        else{
            // Debug.Log("init targets");
            findEmTargets(emAbilityObjectsChild);
        }

        emAbilityObjectsChild = emAbilityObjects.transform.GetChild(1);
        if (emAbilityObjectsChild.name == "EmAbilityZones"){
			// Debug.Log("init zones");
            findEmZones(emAbilityObjectsChild);
        }
        else{
			// Debug.Log("init targets");
            findEmTargets(emAbilityObjectsChild);
        }
    }

    // Update is called once per frame
    void Update () {
		if(RandomMatchmaker.instance && RandomMatchmaker.instance.singlePlayer) {
			enabled = false;
			return;
		}
        if (isEnabled){
            if (Input.GetKeyDown(KeyCode.Q) && isEnabled){
                usingAbility = !usingAbility;
                if (usingAbility){
                    mainCam.enabled = false;
                    emAbilityCam.enabled = true;
                }
                else{
                    mainCam.enabled = true;
                    emAbilityCam.enabled = false;
                }
            }

            if (usingAbility && isEnabled){
                if (getZone() == -1){
                    mainCam.enabled = true;
                    emAbilityCam.enabled = false;
                    usingAbility = false;
                }
                else{
                    emAbilityCam.transform.LookAt(emAbilityTargets[getZone()]);
                    emAbilityCam.transform.position = mainCam.transform.position;
                }
            }
        }
        else{
            mainCam.enabled = true;
            emAbilityCam.enabled = false;
            usingAbility = false;
        }
    }

    void OnTriggerEnter(Collider collider){
        for(int i = 0; i < emAbilityZones.Length; i++){
            if(collider == emAbilityZones[i]){
                inZone[i] = true;
				// Debug.Log("Entered zone " + i);
            }
        }
    }

    void OnTriggerExit(Collider collider){
        for (int i = 0; i < emAbilityZones.Length; i++){
            if (collider == emAbilityZones[i]){
                inZone[i] = false;
                // Debug.Log("Exited zone " + i);
            }
        }
    }

    void findEmZones(Transform emZones){
        emAbilityZones = new Collider[emZones.childCount];
        // Debug.Log("Zones: " + emZones.childCount);
        inZone = new bool[emZones.childCount];

        for(int i = 0; i < emZones.childCount; i++){
            emAbilityZones[i] = emZones.GetChild(i).GetComponent<Collider>();
            inZone[i] = false;
        }
    }

    void findEmTargets(Transform emTargets){
        emAbilityTargets = new Transform[emTargets.childCount];
        // Debug.Log("Tars: " + emTargets.childCount);
        for(int i = 0; i < emTargets.childCount; i++){
            emAbilityTargets[i] = emTargets.GetChild(i);
        }
    }

    public int getZone(){
        for(int i = 0; i < inZone.Length; i++){
            if (inZone[i]) return i;
        }
        return -1;
    }

    public Quaternion getRotation(){
        return emAbilityCam.transform.rotation;
    }

    public bool isUsingAbility()
    {
        return usingAbility;
    }

    public void changeStatus(bool statusIn){
        isEnabled = statusIn;
    }
}
