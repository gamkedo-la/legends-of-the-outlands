using UnityEngine;
using System.Collections;

public class SpongeBehaviour : MonoBehaviour {
    public bool inWater;
    public float wet;
    float wetter = 0.01f;
    Renderer rend;
    Rigidbody rigid;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (inWater && wet < 1){
            wet += wetter;
        }

        rend.material.color = Color.Lerp(Color.yellow, Color.blue, wet);
        rigid.mass = Mathf.Lerp(1.0f, 3.0f, wet);
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
