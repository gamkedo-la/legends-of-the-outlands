using UnityEngine;
using System.Collections;

public class ScalesPlateWeightCounter : MonoBehaviour {
    ScalesTestAnimatorScript scales;
    public bool isPlate1;

    void Start()
    {
        scales = transform.parent.GetComponent<ScalesTestAnimatorScript>();
    }

    void OnCollisionEnter(Collision collision){
        scales.changeWeight(isPlate1, collision.rigidbody.mass);
    }

    void OnCollisionExit(Collision collision)
    {
        scales.changeWeight(isPlate1, -collision.rigidbody.mass);
    }
}
