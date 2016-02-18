using UnityEngine;
using System.Collections;

public class CarryablePin : CarryableObject
{
    public float distance = 0.5f;
    public float stabDepth = 0.2f;

    public override void dropObject(RaycastHit hit)
    {
        if (hit.distance > distance)
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false; //Turn inertia and gravity back on
            transform.parent = transform.parent.parent;
        }
        else
        {
            GetComponent<Collider>().enabled = true;
            transform.parent = hit.transform;
            transform.forward = hit.normal;
            transform.position = hit.point - transform.forward * stabDepth;
        }
        Debug.Log("Hit a: " + hit.transform.name);
    }
}