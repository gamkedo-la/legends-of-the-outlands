using UnityEngine;
using System.Collections;

public class SpongeBehaviour : MonoBehaviour {
	public bool gotWet; // switched to bool for quicker demonstration and since water no dunkable at this time

	/*public bool inWater;
    public float wet;
    float wetter = 0.01f;*/
    Renderer rend;
    Rigidbody rigid;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
		rend.material.color = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {
        /*if (inWater && wet < 1){
            wet += wetter;
        }

        rend.material.color = Color.Lerp(Color.yellow, Color.blue, wet);
        rigid.mass = Mathf.Lerp(1.0f, 3.0f, wet);*/
	}

    void OnCollisionEnter(Collision collision){
		if(gotWet == false && collision.gameObject.name.Contains("sinkwater")) {
			gotWet = true;
			rend.material.color = Color.blue;
			rigid.mass = 3.0f;
			// transform.localScale *= 1.3f;
		}
        /*if (collision.gameObject.name.Contains("sink")){
            inWater = true;
        }*/
    }

    /*void OnCollisionExit(Collision collision){
        if(collision.gameObject.name.Contains("sink")){
            inWater = false;
        }
    }*/
}
