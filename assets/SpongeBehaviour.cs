using UnityEngine;
using System.Collections;

public class SpongeBehaviour : MonoBehaviour {
    public bool inWater;
    public float wet;
    float wetter = 0.01f;
    Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (inWater && wet < 1){
            wet += wetter;
        }

        rend.material.color = Color.Lerp(Color.yellow, Color.blue, wet);
	}

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name.Contains("sink")){
            inWater = true;
        }
    }

    void OnCollisionExit(Collision collision){
        if(collision.gameObject.name.Contains("sink")){
            inWater = false;
        }
    }
}
