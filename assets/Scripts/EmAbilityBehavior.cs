using UnityEngine;
using System.Collections;

public class EmAbilityBehavior : MonoBehaviour {
    GameObject emAbilityObjects;
    Collider[] emAbilityZones = new Collider[1];
    Transform[] emAbilityTargets = new Transform[1];
    bool[] inZone;
    bool abilityButtonDown;

	// Use this for initialization
	void Start () {
        emAbilityObjects = GameObject.FindWithTag("EmAbilityObject");

        Transform emAbilityObjectsChild = emAbilityObjects.transform.GetChild(0);
        if(emAbilityObjectsChild.name == "EmAbilityZones"){
            Debug.Log("init zones");
            findEmZones(emAbilityObjectsChild);
        }
        else{
            Debug.Log("init targets");
            findEmTargets(emAbilityObjectsChild);
        }

        emAbilityObjectsChild = emAbilityObjects.transform.GetChild(1);
        if (emAbilityObjectsChild.name == "EmAbilityZones")
        {
            Debug.Log("init zones");
            findEmZones(emAbilityObjectsChild);
        }
        else
        {
            Debug.Log("init targets");
            findEmTargets(emAbilityObjectsChild);
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.Q))
        {
            abilityButtonDown = true;
        }
        else
        {
            abilityButtonDown = false;
        }
	}

    void OnTriggerEnter(Collider collider){
        for(int i = 0; i < emAbilityZones.Length; i++)
        {
            if(collider == emAbilityZones[i])
            {
                inZone[i] = true;
                Debug.Log("Entered zone " + i);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        for (int i = 0; i < emAbilityZones.Length; i++)
        {
            if (collider == emAbilityZones[i])
            {
                inZone[i] = false;
                Debug.Log("Exited zone " + i);
            }
        }
    }

    void findEmZones(Transform emZones)
    {
        emAbilityZones = new Collider[emZones.childCount];
        inZone = new bool[emZones.childCount];

        for(int i = 0; i < emZones.childCount; i++)
        {
            emAbilityZones[i] = emZones.GetChild(i).GetComponent<Collider>();
            inZone[i] = false;
        }
    }

    void findEmTargets(Transform emTargets)
    {
        emAbilityTargets = new Transform[emTargets.childCount];
        for(int i = 0; i < emTargets.childCount; i++)
        {
            emAbilityTargets[i] = emTargets.GetChild(i);
        }
    }
}
